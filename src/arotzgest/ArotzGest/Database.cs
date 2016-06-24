/*  ArotzGest
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programación: Jorge Moreiro
 *                Iker Estébanez
 * 
 *  Este fichero forma parte de ArotzGest
 *
 * ArotzGest es software libre; puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versión 2 de la licencia o
 * (según su elección) cualquier versión posterior.
 * 
 * ArotzGest es redistribuido con la intención que sea útil, pero SIN NINGUNA
 * GARANTÍA, ni tan solo las garantías implícitas de MERCANTABILIDA o ADECUACIÓN 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para más detalles.
 * 
 * Debería haber recibido una copia de la GNU General Public License acompañando a 
 * ArotzGest.
 * 
 * ÉSTE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - más información en http://www.spri.es
 * 
 *
 * */

using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text.RegularExpressions;

static class Database {
  static OleDbConnection connection;
  public static string [] BudgetStates = new string [] { "Pendiente", "Aceptado", "Rechazado" };
  public static string [] InvoiceStates = new string [] { "Pendiente", "Cobrada" };
  public static string [] LogoTypes = new string [] { "", "Pequeño", "Grande" };
  public static string [] MaterialTypes = new string [] { "Unitario", "Unidimensional", "Bidimensional", "Tridimensional" };
  public static string [] Months = new string [] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
  public static bool Connect () {
    connection = new OleDbConnection ("Provider = Microsoft.Jet.OleDb.4.0; Data Source = " + System.Windows.Forms.Application.StartupPath + "\\ArotzGest.mdb");
    try { connection.Open (); } catch { return false; }
    return true;
  }
  static string escape (string Input) {
    return (Input.Replace ("\"", "\"\""));
  }
  public class Account {
    static string source = "Id, Code, Name from Accounts";
    public readonly int Id;
    public string Code, Name;
    public Account (string code, string name) {
      Code = code;
      Name = name;
    }
    public Account (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Code = dataReader.GetString (1);
      Name = dataReader.GetString (2);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from AccountEntries where AccountId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Code + " - " + Name;
    }
    public Account Update (Account criteria) {
      new OleDbCommand ("update Accounts set Code = \"" + escape (criteria.Code) + "\", Name = \"" + escape (criteria.Name) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Account excluded, string code) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Accounts where Code = \"" + escape (code) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Account Create (Account criteria) {
      new OleDbCommand ("insert into Accounts (Code, Name) values (\"" + escape (criteria.Code) + "\", \"" + escape (criteria.Name) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Accounts where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Account Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Account (dataReader);
      } else return null;
    }
    static List<Account> search (string condition, string order) {
      List<Account> list = new List<Account> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Account (dataReader));
      return list;
    }
    public static List<Account> List () {
      return search ("", "Code");
    }
    public static List<Account> Search (string code, string name) {
      List<string> criteria = new List<string> ();
      if (code != "") criteria.Add ("Code like \"%" + escape (code) + "%\"");
      if (name != "") criteria.Add ("Name like \"%" + escape (name) + "%\"");
      return search (String.Join (" and ", criteria.ToArray ()), "Code");
    }
    public static List<Concept> Balance (string prefix, int year) {
      List<Concept> list = new List<Concept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select Id, Code, Name from Accounts where Code like \"" + escape (prefix) + "%\" order by Code", connection).ExecuteReader ();
      while (dataReader.Read ()) {
        int id = dataReader.GetInt32 (0);
        string code = dataReader.GetString (1);
        string name = dataReader.GetString (2);
        string code2 = Regex.Match (code, "^([0-9]*?)0*$").Groups [1].Value;
        object scalar = new OleDbCommand ("select sum(AccountEntries.Price) from Accounts inner join AccountEntries on Accounts.Id = AccountEntries.AccountId where " + (year == 0 ? "" : "year(AccountEntries.Date) = " + year + " and ") + "Accounts.Code like \"" + escape (code2) + "%\"", connection).ExecuteScalar ();
        double price = scalar.GetType () == typeof (DBNull) ? 0 : (double) scalar;
        list.Add (new Concept (id, code + " - " + name, 0, 0, price));
      }
      return list;
    }
  }
  public class AccountEntry {
    static string source = "AccountEntries.Id, AccountEntries.Name, AccountEntries.AccountId, Accounts.Code, Accounts.Name, AccountEntries.Date, AccountEntries.Price from AccountEntries inner join Accounts on AccountEntries.AccountId = Accounts.Id";
    public readonly int Id;
    public string Name;
    public int AccountId;
    public readonly string AccountCode, AccountName;
    public DateTime Date;
    public double Price;
    public AccountEntry (string name, int accountId, DateTime date, double price) {
      Name = name;
      AccountId = accountId;
      Date = date;
      Price = price;
    }
    public AccountEntry (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      AccountId = dataReader.GetInt32 (2);
      AccountCode = dataReader.GetString (3);
      AccountName = dataReader.GetString (4);
      Date = dataReader.GetDateTime (5);
      Price = dataReader.GetDouble (6);
    }
    public AccountEntry Update (AccountEntry criteria) {
      new OleDbCommand ("update AccountEntries set Name = \"" + escape (criteria.Name) + "\", AccountId = " + criteria.AccountId + ", [Date] = \"" + Data.Date_Format (criteria.Date) + "\", Price = \"" + criteria.Price + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static AccountEntry Create (AccountEntry criteria) {
      new OleDbCommand ("insert into AccountEntries (Name, AccountId, [Date], Price) values (\"" + escape (criteria.Name) + "\", " + criteria.AccountId + ", \"" + Data.Date_Format (criteria.Date) + "\", \"" + criteria.Price + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from AccountEntries where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static AccountEntry Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where AccountEntries.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new AccountEntry (dataReader);
      } else return null;
    }
    static List<AccountEntry> search (string condition, string order) {
      List<AccountEntry> list = new List<AccountEntry> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new AccountEntry (dataReader));
      return list;
    }
    public static List<AccountEntry> Search (int accountId, int month, int year) {
      List<string> criteria = new List<string> ();
      if (accountId > 0) criteria.Add ("AccountEntries.AccountId = " + accountId);
      if (month > 0) criteria.Add ("month(AccountEntries.Date) = " + month);
      if (year > 0) criteria.Add ("year(AccountEntries.Date) = " + year);
      return search (String.Join (" and ", criteria.ToArray ()), "Accounts.Code, AccountEntries.Date");
    }
  }
  public class Budget {
    static string source = "Budgets.Id, Budgets.Number, Budgets.ClientId, Clients.Name, Budgets.Date, Budgets.State from Budgets inner join Clients on Budgets.ClientId = Clients.Id";
    public readonly int Id;
    public int Number;
    public int ClientId;
    public readonly string ClientName;
    public DateTime Date;
    public int State;
    public double Cost, Price;
    public Budget (int number, int clientId, DateTime date, int state) {
      Number = number;
      ClientId = clientId;
      Date = date;
      State = state;
    }
    public Budget (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Number = dataReader.GetInt32 (1);
      ClientId = dataReader.GetInt32 (2);
      ClientName = dataReader.GetString (3);
      Date = dataReader.GetDateTime (4);
      State = dataReader.GetInt32 (5);
    }
    public static int CalculateNumber () {
      object scalar = new OleDbCommand ("select max (Number) from Budgets", connection).ExecuteScalar ();
      return scalar.GetType () == typeof (DBNull) ? 1 : (int) scalar + 1;
    }
    public void Calculate () {
      Cost = 0;
      Price = 0;
      foreach (string table in new string [] { "BudgetTemplates", "BudgetArbitraries" }) {
        OleDbDataReader dataReader = new OleDbCommand ("select Quantity, Cost, Price from " + table + " where BudgetId = " + Id, connection).ExecuteReader ();
        while (dataReader.Read ()) {
          Cost += dataReader.GetDouble (0) * dataReader.GetDouble (1);
          Price += dataReader.GetDouble (0) * dataReader.GetDouble (2);
        }
      }
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 InvoiceId from InvoiceBudgets where BudgetId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Number.ToString ();
    }
    public Budget Update (Budget criteria) {
      new OleDbCommand ("update Budgets set [Number] = " + criteria.Number + ", ClientId = " + criteria.ClientId + ", [Date] = \"" + Data.Date_Format (criteria.Date) + "\", State = " + criteria.State + " where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Budget excluded, int number) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Budgets where [Number] = " + number + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Budget Create (Budget criteria) {
      new OleDbCommand ("insert into Budgets ([Number], ClientId, [Date], State) values (" + criteria.Number + ", " + criteria.ClientId + ", \"" + Data.Date_Format (criteria.Date) + "\", " + criteria.State + ")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Budgets where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Budget Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Budgets.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Budget (dataReader);
      } else return null;
    }
    static List<Budget> search (string condition, string order) {
      List<Budget> list = new List<Budget> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Budget (dataReader));
      return list;
    }
    public static List<Budget> Search (int clientId, int month, int year, int state) {
      List<string> criteria = new List<string> ();
      if (clientId > 0) criteria.Add ("Budgets.ClientId = " + clientId);
      if (month > 0) criteria.Add ("month(Budgets.Date) = " + month);
      if (year > 0) criteria.Add ("year(Budgets.Date) = " + year);
      if (state >= 0) criteria.Add ("state = " + state);
      return search (String.Join (" and ", criteria.ToArray ()), "Budgets.Number");
    }
    public static List<Budget> ListPendant () {
      return search ("Budgets.State = 0", "Budgets.Number");
    }
    public static List<Budget> ListAccepted () {
      return search ("Budgets.State = 1 and Budgets.Id not in (select InvoiceBudgets.BudgetId from InvoiceBudgets)", "Budgets.Number");
    }
  }
  public class Client {
    static string source = "Id, Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes from Clients";
    public readonly int Id;
    public string Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes;
    public Client (string name, string fic, string address, string city, string province, string zip, string phone, string mobile, string email, string homepage, string account, string notes) {
      Name = name;
      FIC = fic;
      Address = address;
      City = city;
      Province = province;
      ZIP = zip;
      Phone = phone;
      Mobile = mobile;
      Email = email;
      Homepage = homepage;
      Account = account;
      Notes = notes;
    }
    public Client (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      FIC = dataReader.GetString (2);
      Address = dataReader.GetString (3);
      City = dataReader.GetString (4);
      Province = dataReader.GetString (5);
      ZIP = dataReader.GetString (6);
      Phone = dataReader.GetString (7);
      Mobile = dataReader.GetString (8);
      Email = dataReader.GetString (9);
      Homepage = dataReader.GetString (10);
      Account = dataReader.GetString (11);
      Notes = dataReader.GetString (12);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select Id from Budgets where clientId = " + Id + " union select Id from Invoices where ClientId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Name;
    }
    public Client Update (Client criteria) {
      new OleDbCommand ("update Clients set Name = \"" + escape (criteria.Name) + "\", FIC = \"" + escape (criteria.FIC) + "\", Address = \"" + escape (criteria.Address) + "\", City = \"" + escape (criteria.City) + "\", Province = \"" + escape (criteria.Province) + "\", ZIP = \"" + escape (criteria.ZIP) + "\", Phone = \"" + escape (criteria.Phone) + "\", Mobile = \"" + escape (criteria.Mobile) + "\", Email = \"" + escape (criteria.Email) + "\", Homepage = \"" + escape (criteria.Homepage) + "\", Account = \"" + escape (criteria.Account) + "\", Notes = \"" + escape (criteria.Notes) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Client excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Clients where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Client Create (Client criteria) {
      new OleDbCommand ("insert into Clients (Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes) values (\"" + escape (criteria.Name) + "\", \"" + escape (criteria.FIC) + "\", \"" + escape (criteria.Address) + "\", \"" + escape (criteria.City) + "\", \"" + escape (criteria.Province) + "\", \"" + escape (criteria.ZIP) + "\", \"" + escape (criteria.Phone) + "\", \"" + escape (criteria.Mobile) + "\", \"" + escape (criteria.Email) + "\", \"" + escape (criteria.Homepage) + "\", \"" + escape (criteria.Account) + "\", \"" + escape (criteria.Notes) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Clients where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Client Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Client (dataReader);
      } else return null;
    }
    static List<Client> search (string condition, string order) {
      List<Client> list = new List<Client> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Client (dataReader));
      return list;
    }
    public static List<Client> List () {
      return search ("", "Name");
    }
    public static List<Client> Search (string name) {
      return search ("Name like \"%" + escape (name) + "%\"", "Name");
    }
  }
  public class Company {
    public static string Name, FIC, Address, City, Province, ZIP, LogoImage;
    public static int BudgetValidDays, LogoType;
    public static void Get () {
      OleDbDataReader dataReader = new OleDbCommand ("select Name, FIC, Address, City, Province, ZIP, BudgetValidDays, LogoType, LogoImage from Company", connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        Name = dataReader.GetString (0);
        FIC = dataReader.GetString (1);
        Address = dataReader.GetString (2);
        City = dataReader.GetString (3);
        Province = dataReader.GetString (4);
        ZIP = dataReader.GetString (5);
        BudgetValidDays = dataReader.GetInt32 (6);
        LogoType = dataReader.GetInt32 (7);
        LogoImage = dataReader.GetString (8);
      }
    }
    public static void Update (string name, string fic, string address, string city, string province, string zip, int budgetValidDays, int logoType, string logoImage) {
      if (new OleDbCommand ("select * from Company", connection).ExecuteReader ().HasRows) new OleDbCommand ("update Company set Name = \"" + escape (name) + "\", FIC = \"" + escape (fic) + "\", Address = \"" + escape (address) + "\", City = \"" + escape (city) + "\", Province = \"" + escape (province) + "\", ZIP = \"" + escape (zip) + "\", BudgetValidDays = " + budgetValidDays + ", LogoType = " + logoType + ", LogoImage = \"" + escape (logoImage) + "\"", connection).ExecuteReader ();
      else new OleDbCommand ("insert into Company (Name, FIC, Address, City, Province, ZIP, BudgetValidDays, LogoType, LogoImage) values (\"" + escape (name) + "\", \"" + escape (fic) + "\", \"" + escape (address) + "\", \"" + escape (city) + "\", \"" + escape (province) + "\", \"" + escape (zip) + "\", " + budgetValidDays + ", " + logoType + " , \"" + escape (logoImage) + "\")", connection).ExecuteReader ();
    }
  }
  public class Employee {
    static string source = "Employees.Id, Employees.Name, Employees.CategoryId, EmployeeCategories.Name, Employees.FIN, Employees.SSN, Employees.Address, Employees.City, Employees.Province, Employees.ZIP, Employees.Phone, Employees.Mobile, Employees.Email, Employees.Account, Employees.Notes from Employees inner join EmployeeCategories on Employees.CategoryId = EmployeeCategories.Id";
    public readonly int Id;
    public string Name;
    public int CategoryId;
    public readonly string CategoryName;
    public string FIN, SSN, Address, City, Province, ZIP, Phone, Mobile, Email, Account, Notes;
    public Employee (string name, int categoryId, string fin, string ssn, string address, string city, string province, string zip, string phone, string mobile, string email, string account, string notes) {
      Name = name;
      CategoryId = categoryId;
      FIN = fin;
      SSN = ssn;
      Address = address;
      City = city;
      Province = province;
      ZIP = zip;
      Phone = phone;
      Mobile = mobile;
      Email = email;
      Account = account;
      Notes = notes;
    }
    public Employee (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      CategoryId = dataReader.GetInt32 (2);
      CategoryName = dataReader.GetString (3);
      FIN = dataReader.GetString (4);
      SSN = dataReader.GetString (5);
      Address = dataReader.GetString (6);
      City = dataReader.GetString (7);
      Province = dataReader.GetString (8);
      ZIP = dataReader.GetString (9);
      Phone = dataReader.GetString (10);
      Mobile = dataReader.GetString (11);
      Email = dataReader.GetString (12);
      Account = dataReader.GetString (13);
      Notes = dataReader.GetString (14);
    }
    public bool CheckUsed () {
      return false;
    }
    public Employee Update (Employee criteria) {
      new OleDbCommand ("update Employees set Name = \"" + escape (criteria.Name) + "\", CategoryId = " + criteria.CategoryId + ", FIN = \"" + escape (criteria.FIN) + "\", SSN = \"" + escape (criteria.SSN) + "\", Address = \"" + escape (criteria.Address) + "\", City = \"" + escape (criteria.City) + "\", Province = \"" + escape (criteria.Province) + "\", ZIP = \"" + escape (criteria.ZIP) + "\", Phone = \"" + escape (criteria.Phone) + "\", Mobile = \"" + escape (criteria.Mobile) + "\", Email = \"" + escape (criteria.Email) + "\", Account = \"" + escape (criteria.Account) + "\", Notes = \"" + escape (criteria.Notes) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Employee excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Employees where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Employee Create (Employee criteria) {
      new OleDbCommand ("insert into Employees (Name, CategoryId, FIN, SSN, Address, City, Province, ZIP, Phone, Mobile, Email, Account, Notes) values (\"" + escape (criteria.Name) + "\", " + criteria.CategoryId + ", \"" + escape (criteria.FIN) + "\", \"" + escape (criteria.SSN) + "\", \"" + escape (criteria.Address) + "\", \"" + escape (criteria.City) + "\", \"" + escape (criteria.Province) + "\", \"" + escape (criteria.ZIP) + "\", \"" + escape (criteria.Phone) + "\", \"" + escape (criteria.Mobile) + "\", \"" + escape (criteria.Email) + "\", \"" + escape (criteria.Account) + "\", \"" + escape (criteria.Notes) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Employees where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Employee Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Employees.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Employee (dataReader);
      } else return null;
    }
    static List<Employee> search (string condition, string order) {
      List<Employee> list = new List<Employee> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Employee (dataReader));
      return list;
    }
    public static List<Employee> Search (string name, int categoryId) {
      List<string> criteria = new List<string> ();
      if (name != "") criteria.Add ("Employees.Name like \"%" + escape (name) + "%\"");
      if (categoryId > 0) criteria.Add ("Employees.CategoryId = " + categoryId);
      return search (String.Join (" and ", criteria.ToArray ()), "Employees.Name");
    }
  }
  public class EmployeeCategory {
    static string source = "Id, Name, Cost, Price from EmployeeCategories";
    public readonly int Id;
    public string Name;
    public double Cost;
    public double Price;
    public EmployeeCategory (string name, double cost, double price) {
      Name = name;
      Cost = cost;
      Price = price;
    }
    public EmployeeCategory (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      Cost = dataReader.GetDouble (2);
      Price = dataReader.GetDouble (3);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Employees where CategoryId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Name;
    }
    public EmployeeCategory Update (EmployeeCategory criteria) {
      new OleDbCommand ("update EmployeeCategories set Name = \"" + escape (criteria.Name) + "\", Cost = \"" + criteria.Cost + "\", Price = \"" + criteria.Price + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public void UpdatePrices (double cost, double price) {
      new OleDbCommand ("update EmployeeCategories set Cost = \"" + cost + "\", Price = \"" + price + "\" where Id = " + Id, connection).ExecuteNonQuery ();
    }
    public static bool CheckUnique (EmployeeCategory excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from EmployeeCategories where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static EmployeeCategory Create (EmployeeCategory criteria) {
      new OleDbCommand ("insert into EmployeeCategories (Name, Cost, Price) values (\"" + escape (criteria.Name) + "\", \"" + criteria.Cost + "\", \"" + criteria.Price + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from EmployeeCategories where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static EmployeeCategory Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new EmployeeCategory (dataReader);
      } else return null;
    }
    static List<EmployeeCategory> search (string condition, string order) {
      List<EmployeeCategory> list = new List<EmployeeCategory> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new EmployeeCategory (dataReader));
      return list;
    }
    public static List<EmployeeCategory> List () {
      return search ("", "Name");
    }
    public static List<EmployeeCategory> Search (string name) {
      return search ("Name like \"%" + escape (name) + "%\"", "Name");
    }
  }
  public class Invoice {
    static string source = "Invoices.Id, Invoices.Number, Invoices.ClientId, Clients.Name, Invoices.Date, Invoices.VAT, Invoices.State from Invoices inner join Clients on Invoices.ClientId = Clients.Id";
    public readonly int Id;
    public int Number;
    public int ClientId;
    public readonly string ClientName;
    public DateTime Date;
    public int VAT;
    public int State;
    public double Cost, Price;
    public Invoice (int number, int clientId, DateTime date, int vat, int state) {
      Number = number;
      ClientId = clientId;
      Date = date;
      VAT = vat;
      State = state;
    }
    public Invoice (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Number = dataReader.GetInt32 (1);
      ClientId = dataReader.GetInt32 (2);
      ClientName = dataReader.GetString (3);
      Date = dataReader.GetDateTime (4);
      VAT = dataReader.GetInt32 (5);
      State = dataReader.GetInt32 (6);
    }
    public static int CalculateNumber () {
      object scalar = new OleDbCommand ("select max (Number) from Invoices", connection).ExecuteScalar ();
      return scalar.GetType () == typeof (DBNull) ? 1 : (int) scalar + 1;
    }
    public void Calculate () {
      Cost = 0;
      Price = 0;
      foreach (string table in new string [] { "InvoiceBudgets", "InvoiceArbitraries" }) {
        OleDbDataReader dataReader = new OleDbCommand ("select Quantity, Cost, Price from " + table + " where InvoiceId = " + Id, connection).ExecuteReader ();
        while (dataReader.Read ()) {
          Cost += dataReader.GetDouble (0) * dataReader.GetDouble (1);
          Price += dataReader.GetDouble (0) * dataReader.GetDouble (2);
        }
      }
    }
    public bool CheckUsed () {
      return false;
    }
    public Invoice Update (Invoice criteria) {
      new OleDbCommand ("update Invoices set [Number] = " + criteria.Number + ", ClientId = " + criteria.ClientId + ", [Date] = \"" + Data.Date_Format (criteria.Date) + "\", VAT = " + criteria.VAT + ", State = " + criteria.State + " where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Invoice excluded, int number) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Invoices where [Number] = " + number + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Invoice Create (Invoice criteria) {
      new OleDbCommand ("insert into Invoices ([Number], ClientId, [Date], VAT, State) values (" + criteria.Number + ", " + criteria.ClientId + ", \"" + Data.Date_Format (criteria.Date) + "\", " + criteria.VAT + ", " + criteria.State + ")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Invoices where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Invoice Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Invoices.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Invoice (dataReader);
      } else return null;
    }
    static List<Invoice> search (string condition, string order) {
      List<Invoice> list = new List<Invoice> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Invoice (dataReader));
      return list;
    }
    public static List<Invoice> Search (int clientId, int month, int year, int state) {
      List<string> criteria = new List<string> ();
      if (clientId > 0) criteria.Add ("Invoices.ClientId = " + clientId);
      if (month > 0) criteria.Add ("month(Invoices.Date) = " + month);
      if (year > 0) criteria.Add ("year(Invoices.Date) = " + year);
      if (state >= 0) criteria.Add ("state = " + state);
      return search (String.Join (" and ", criteria.ToArray ()), "Invoices.Number");
    }
    public static List<Invoice> ListPendant () {
      return search ("Invoices.State = false", "Invoices.Number");
    }
  }
  public class Material {
    static string source = "Materials.Id, Materials.Name, Materials.CategoryId, MaterialCategories.Name, Materials.ProviderId, Providers.Name, Materials.Type, Materials.Dimension1, Materials.Dimension2, Materials.Dimension3, Materials.Cost, Materials.Price from (Materials inner join MaterialCategories on Materials.CategoryId = MaterialCategories.Id) inner join Providers on Materials.ProviderId = Providers.Id";
    public readonly int Id;
    public string Name;
    public int CategoryId;
    public string CategoryName;
    public int ProviderId;
    public string ProviderName;
    public int Type;
    public double Dimension1, Dimension2, Dimension3;
    public double Cost;
    public double Price;
    public Material (string name, int categoryId, int providerId, int type, double dimension1, double dimension2, double dimension3, double cost, double price) {
      Name = name;
      CategoryId = categoryId;
      ProviderId = providerId;
      Type = type;
      Dimension1 = dimension1;
      Dimension2 = dimension2;
      Dimension3 = dimension3;
      Cost = cost;
      Price = price;
    }
    public Material (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      CategoryId = dataReader.GetInt32 (2);
      CategoryName = dataReader.GetString (3);
      ProviderId = dataReader.GetInt32 (4);
      ProviderName = dataReader.GetString (5);
      Type = dataReader.GetInt32 (6);
      Dimension1 = dataReader.GetDouble (7);
      Dimension2 = dataReader.GetDouble (8);
      Dimension3 = dataReader.GetDouble (9);
      Cost = dataReader.GetDouble (10);
      Price = dataReader.GetDouble (11);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 TemplateId from TemplateMaterials where MaterialId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public Material Update (Material criteria) {
      new OleDbCommand ("update Materials set Name = \"" + escape (criteria.Name) + "\", CategoryId = " + criteria.CategoryId + ", ProviderId = " + criteria.ProviderId + ", Type = " + criteria.Type + ", Dimension1 = \"" + criteria.Dimension1 + "\", Dimension2 = \"" + criteria.Dimension2 + "\", Dimension3 = \"" + criteria.Dimension3 + "\", Cost = \"" + criteria.Cost + "\", Price = \"" + criteria.Price + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public void UpdatePrices (double cost, double price) {
      new OleDbCommand ("update Materials set Cost = \"" + cost + "\", Price = \"" + price + "\" where Id = " + Id, connection).ExecuteNonQuery ();
    }
    public static bool CheckUnique (Material excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Materials where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Material Create (Material criteria) {
      new OleDbCommand ("insert into Materials (Name, CategoryId, ProviderId, Type, Dimension1, Dimension2, Dimension3, Cost, Price) values (\"" + escape (criteria.Name) + "\", " + criteria.CategoryId + ", " + criteria.ProviderId + ", " + criteria.Type + ", \"" + criteria.Dimension1 + "\", \"" + criteria.Dimension2 + "\", \"" + criteria.Dimension3 + "\", \"" + criteria.Cost + "\", \"" + criteria.Price + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Materials where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Material Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Materials.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Material (dataReader);
      } else return null;
    }
    static List<Material> search (string condition, string order) {
      List<Material> list = new List<Material> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Material (dataReader));
      return list;
    }
    public static List<Material> List (int categoryId) {
      return search ("Materials.CategoryId = " + categoryId, "Materials.Name");
    }
    public static List<Material> Search (string name, int categoryId, int providerId) {
      List<string> criteria = new List<string> ();
      if (name != "") criteria.Add ("Materials.Name like \"%" + escape (name) + "%\"");
      if (categoryId > 0) criteria.Add ("Materials.CategoryId = " + categoryId);
      if (providerId > 0) criteria.Add ("Materials.ProviderId = " + providerId);
      return search (String.Join (" and ", criteria.ToArray ()), "MaterialCategories.Name, Materials.Name");
    }
  }
  public class MaterialCategory {
    static string source = "Id, Name from MaterialCategories";
    public readonly int Id;
    public string Name;
    public MaterialCategory (string name) {
      Name = name;
    }
    public MaterialCategory (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Materials where CategoryId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Name;
    }
    public MaterialCategory Update (MaterialCategory criteria) {
      new OleDbCommand ("update MaterialCategories set Name = \"" + escape (criteria.Name) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (MaterialCategory excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from MaterialCategories where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static MaterialCategory Create (MaterialCategory criteria) {
      new OleDbCommand ("insert into MaterialCategories (Name) values (\"" + escape (criteria.Name) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from MaterialCategories where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static MaterialCategory Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new MaterialCategory (dataReader);
      } else return null;
    }
    static List<MaterialCategory> search (string condition, string order) {
      List<MaterialCategory> list = new List<MaterialCategory> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new MaterialCategory (dataReader));
      return list;
    }
    public static List<MaterialCategory> List () {
      return search ("", "Name");
    }
    public static List<MaterialCategory> Search (string name) {
      return search ("Name like \"%" + escape (name) + "%\"", "Name");
    }
  }
  public class Provider {
    static string source = "Id, Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes from Providers";
    public readonly int Id;
    public string Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes;
    public Provider (string name, string fic, string address, string city, string province, string zip, string phone, string mobile, string email, string homepage, string account, string notes) {
      Name = name;
      FIC = fic;
      Address = address;
      City = city;
      Province = province;
      ZIP = zip;
      Phone = phone;
      Mobile = mobile;
      Email = email;
      Homepage = homepage;
      Account = account;
      Notes = notes;
    }
    public Provider (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      FIC = dataReader.GetString (2);
      Address = dataReader.GetString (3);
      City = dataReader.GetString (4);
      Province = dataReader.GetString (5);
      ZIP = dataReader.GetString (6);
      Phone = dataReader.GetString (7);
      Mobile = dataReader.GetString (8);
      Email = dataReader.GetString (9);
      Homepage = dataReader.GetString (10);
      Account = dataReader.GetString (11);
      Notes = dataReader.GetString (12);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Materials where ProviderId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Name;
    }
    public Provider Update (Provider criteria) {
      new OleDbCommand ("update Providers set Name = \"" + escape (criteria.Name) + "\", FIC = \"" + escape (criteria.FIC) + "\", Address = \"" + escape (criteria.Address) + "\", City = \"" + escape (criteria.City) + "\", Province = \"" + escape (criteria.Province) + "\", ZIP = \"" + escape (criteria.ZIP) + "\", Phone = \"" + escape (criteria.Phone) + "\", Mobile = \"" + escape (criteria.Mobile) + "\", Email = \"" + escape (criteria.Email) + "\", Homepage = \"" + escape (criteria.Homepage) + "\", Account = \"" + escape (criteria.Account) + "\", Notes = \"" + escape (criteria.Notes) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (Provider excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Providers where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Provider Create (Provider criteria) {
      new OleDbCommand ("insert into Providers (Name, FIC, Address, City, Province, ZIP, Phone, Mobile, Email, Homepage, Account, Notes) values (\"" + escape (criteria.Name) + "\", \"" + escape (criteria.FIC) + "\", \"" + escape (criteria.Address) + "\", \"" + escape (criteria.City) + "\", \"" + escape (criteria.Province) + "\", \"" + escape (criteria.ZIP) + "\", \"" + escape (criteria.Phone) + "\", \"" + escape (criteria.Mobile) + "\", \"" + escape (criteria.Email) + "\", \"" + escape (criteria.Homepage) + "\", \"" + escape (criteria.Account) + "\", \"" + escape (criteria.Notes) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Providers where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Provider Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Provider (dataReader);
      } else return null;
    }
    static List<Provider> search (string condition, string order) {
      List<Provider> list = new List<Provider> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Provider (dataReader));
      return list;
    }
    public static List<Provider> List () {
      return search ("", "Name");
    }
    public static List<Provider> Search (string name) {
      return search ("Name like \"%" + escape (name) + "%\"", "Name");
    }
  }
  public class Template {
    static string source = "Templates.Id, Templates.Name, Templates.CategoryId, TemplateCategories.Name, Templates.Description from Templates inner join TemplateCategories on Templates.CategoryId = TemplateCategories.Id";
    public readonly int Id;
    public string Name;
    public int CategoryId;
    public readonly string CategoryName;
    public string Description;
    public double Cost;
    public double Price;
    public Template (string name, int categoryId, string description) {
      Name = name;
      CategoryId = categoryId;
      Description = description;
    }
    public Template (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      CategoryId = dataReader.GetInt32 (2);
      CategoryName = dataReader.GetString (3);
      Description = dataReader.GetString (4);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 BudgetId from BudgetTemplates where TemplateId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public Template Update (Template criteria) {
      new OleDbCommand ("update Templates set Name = \"" + escape (criteria.Name) + "\", CategoryId = " + criteria.CategoryId + ", Description = \"" + escape (criteria.Description) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public void Calculate () {
      Cost = 0;
      Price = 0;
      foreach (string table in new string [] { "TemplateEmployeeCategories", "TemplateMaterials", "TemplateArbitraries" }) {
        OleDbDataReader dataReader = new OleDbCommand ("select Quantity, Cost, Price from " + table + " where TemplateId = " + Id, connection).ExecuteReader ();
        while (dataReader.Read ()) {
          Cost += dataReader.GetDouble (0) * dataReader.GetDouble (1);
          Price += dataReader.GetDouble (0) * dataReader.GetDouble (2);
        }
      }
    }
    public static bool CheckUnique (Template excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Templates where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static Template Create (Template criteria) {
      new OleDbCommand ("insert into Templates (Name, CategoryId, Description) values (\"" + escape (criteria.Name) + "\", " + criteria.CategoryId + ", \"" + escape (criteria.Description) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from Templates where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static Template Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Templates.Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new Template (dataReader);
      } else return null;
    }
    static List<Template> search (string condition, string order) {
      List<Template> list = new List<Template> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Template (dataReader));
      return list;
    }
    public static List<Template> List (int categoryId) {
      return search ("Templates.CategoryId = " + categoryId, "Templates.Name");
    }
    public static List<Template> Search (string name, int categoryId) {
      List<string> criteria = new List<string> ();
      if (name != "") criteria.Add ("Templates.Name like \"%" + escape (name) + "%\"");
      if (categoryId > 0) criteria.Add ("Templates.CategoryId = " + categoryId);
      return search (String.Join (" and ", criteria.ToArray ()), "Templates.Name");
    }
  }
  public class TemplateCategory {
    static string source = "Id, Name from TemplateCategories";
    public readonly int Id;
    public string Name;
    public TemplateCategory (string name) {
      Name = name;
    }
    public TemplateCategory (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
    }
    public bool CheckUsed () {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from Templates where CategoryId = " + Id, connection).ExecuteReader ();
      return dataReader.HasRows;
    }
    public override string ToString () {
      return Name;
    }
    public TemplateCategory Update (TemplateCategory criteria) {
      new OleDbCommand ("update TemplateCategories set Name = \"" + escape (criteria.Name) + "\" where Id = " + Id, connection).ExecuteNonQuery ();
      return Get (Id);
    }
    public static bool CheckUnique (TemplateCategory excluded, string name) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 Id from TemplateCategories where Name = \"" + escape (name) + "\"" + (excluded != null ? " and not Id = " + excluded.Id : ""), connection).ExecuteReader ();
      return !dataReader.HasRows;
    }
    public static TemplateCategory Create (TemplateCategory criteria) {
      new OleDbCommand ("insert into TemplateCategories (Name) values (\"" + escape (criteria.Name) + "\")", connection).ExecuteNonQuery ();
      return Get ((int) new OleDbCommand ("select @@identity", connection).ExecuteScalar ());
    }
    public static void Delete (int id) {
      new OleDbCommand ("delete from TemplateCategories where Id = " + id, connection).ExecuteNonQuery ();
    }
    public static TemplateCategory Get (int id) {
      OleDbDataReader dataReader = new OleDbCommand ("select top 1 " + source + " where Id = " + id, connection).ExecuteReader ();
      if (dataReader.HasRows) {
        dataReader.Read ();
        return new TemplateCategory (dataReader);
      } else return null;
    }
    static List<TemplateCategory> search (string condition, string order) {
      List<TemplateCategory> list = new List<TemplateCategory> ();
      OleDbDataReader dataReader = new OleDbCommand ("select " + source + " " + (condition != "" ? " where " + condition : "") + " order by " + order, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new TemplateCategory (dataReader));
      return list;
    }
    public static List<TemplateCategory> List () {
      return search ("", "Name");
    }
    public static List<TemplateCategory> Search (string name) {
      return search ("Name like \"%" + escape (name) + "%\"", "Name");
    }
  }
  public class Concept {
    public int Id;
    public string Name;
    public double Quantity;
    public double Cost;
    public double Price;
    public Concept (int id, string name, double quantity, double cost, double price) {
      Id = id;
      Name = name;
      Quantity = quantity;
      Cost = cost;
      Price = price;
    }
    public Concept (OleDbDataReader dataReader) {
      Id = dataReader.GetInt32 (0);
      Name = dataReader.GetString (1);
      Quantity = dataReader.GetDouble (2);
      Cost = dataReader.GetDouble (3);
      Price = dataReader.GetDouble (4);
    }
    public static void DeleteForBudget (int budgetId) {
      new OleDbCommand ("delete from BudgetTemplates where BudgetId = " + budgetId, connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from BudgetArbitraries where BudgetId = " + budgetId, connection).ExecuteNonQuery ();
    }
    public static void DeleteForInvoice (int invoiceId) {
      new OleDbCommand ("delete from InvoiceBudgets where InvoiceId = " + invoiceId, connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from InvoiceArbitraries where InvoiceId = " + invoiceId, connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from AccountEntries where id in (select AccountEntryId from InvoiceReceipts where InvoiceId = " + invoiceId + ")", connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from InvoiceReceipts where InvoiceId = " + invoiceId, connection).ExecuteNonQuery ();
    }
    public static void DeleteForTemplate (int templateId) {
      new OleDbCommand ("delete from TemplateEmployeeCategories where TemplateId = " + templateId, connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from TemplateMaterials where TemplateId = " + templateId, connection).ExecuteNonQuery ();
      new OleDbCommand ("delete from TemplateArbitraries where TemplateId = " + templateId, connection).ExecuteNonQuery ();
    }
    public virtual void CreateForBudget (int budgetId) { }
    public virtual void CreateForInvoice (int invoiceId) { }
    public virtual void CreateForTemplate (int templateId) { }
  }
  public class EmployeeCategoryConcept : Concept {
    public EmployeeCategoryConcept (int id, string name, double quantity, double cost, double price) : base (id, name, quantity, cost, price) { }
    public EmployeeCategoryConcept (OleDbDataReader dataReader) : base (dataReader) {}
    public static List<EmployeeCategoryConcept> GetForTemplate (int templateId) {
      List<EmployeeCategoryConcept> list = new List<EmployeeCategoryConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select TemplateEmployeeCategories.EmployeeCategoryId, EmployeeCategories.Name, TemplateEmployeeCategories.Quantity, TemplateEmployeeCategories.Cost, TemplateEmployeeCategories.Price from TemplateEmployeeCategories inner join EmployeeCategories on TemplateEmployeeCategories.EmployeeCategoryId = EmployeeCategories.Id where TemplateEmployeeCategories.TemplateId = " + templateId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new EmployeeCategoryConcept (dataReader));
      return list;
    }
    public override void CreateForTemplate (int templateId) {
      new OleDbCommand ("insert into TemplateEmployeeCategories (TemplateId, EmployeeCategoryId, Quantity, Cost, Price) values (" + templateId + ", " + Id + ", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
  }
  public class MaterialConcept : Concept {
    public int Type;
    public double Dimension1, Dimension2, Dimension3;
    public MaterialConcept (int id, string name, int type, double dimension1, double dimension2, double dimension3, double quantity, double cost, double price) : base (id, name, quantity, cost, price) {
      Type = type;
      Dimension1 = dimension1;
      Dimension2 = dimension2;
      Dimension3 = dimension3;
    }
    public MaterialConcept (OleDbDataReader dataReader) : base (dataReader) {
      Type = dataReader.GetInt32 (5);
      Dimension1 = dataReader.GetDouble (6);
      Dimension2 = dataReader.GetDouble (7);
      Dimension3 = dataReader.GetDouble (8);
    }
    public static List<MaterialConcept> GetForTemplate (int templateId) {
      List<MaterialConcept> list = new List<MaterialConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select TemplateMaterials.MaterialId, Materials.Name, TemplateMaterials.Quantity, TemplateMaterials.Cost, TemplateMaterials.Price, TemplateMaterials.Type, TemplateMaterials.Dimension1, TemplateMaterials.Dimension2, TemplateMaterials.Dimension3 from TemplateMaterials inner join Materials on TemplateMaterials.MaterialId = Materials.Id where TemplateMaterials.TemplateId = " + templateId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new MaterialConcept (dataReader));
      return list;
    }
    public override void CreateForTemplate (int templateId) {
      new OleDbCommand ("insert into TemplateMaterials (TemplateId, MaterialId, Type, Dimension1, Dimension2, Dimension3, Quantity, Cost, Price) values (" + templateId + ", " + Id + ", " + Type + ", \"" + Dimension1 + "\", \"" + Dimension2 + "\", \"" + Dimension3 + "\", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
    public static List<Concept> StatisticsForTemplate (int templateId) {
      List<Concept> list = new List<Concept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select 0, first(MaterialCategories.Name), cdbl(0), sum(TemplateMaterials.Quantity * TemplateMAterials.Cost), sum(TemplateMaterials.Quantity * TemplateMaterials.Price) from (TemplateMaterials inner join Materials on TemplateMaterials.MaterialId = Materials.Id) inner join MaterialCategories on Materials.CategoryId = MaterialCategories.Id where TemplateMaterials.TemplateId = " + templateId + " group by Materials.CategoryId", connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new Concept (dataReader));
      return list;
    }
  }
  public class ArbitraryConcept : Concept {
    public ArbitraryConcept (string name, double quantity, double cost, double price) : base (0, name, quantity, cost, price) { }
    public ArbitraryConcept (OleDbDataReader dataReader) : base (dataReader) { }
    public static List<ArbitraryConcept> GetForBudget (int budgetId) {
      List<ArbitraryConcept> list = new List<ArbitraryConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select 0, Name, Quantity, Cost, Price from BudgetArbitraries where budgetId = " + budgetId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new ArbitraryConcept (dataReader));
      return list;
    }
    public static List<ArbitraryConcept> GetForInvoice (int invoiceId) {
      List<ArbitraryConcept> list = new List<ArbitraryConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select 0, Name, Quantity, Cost, Price from InvoiceArbitraries where InvoiceId = " + invoiceId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new ArbitraryConcept (dataReader));
      return list;
    }
    public static List<ArbitraryConcept> GetForTemplate (int templateId) {
      List<ArbitraryConcept> list = new List<ArbitraryConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select 0, Name, Quantity, Cost, Price from TemplateArbitraries where TemplateId = " + templateId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new ArbitraryConcept (dataReader));
      return list;
    }
    public override void CreateForBudget (int budgetId) {
      new OleDbCommand ("insert into BudgetArbitraries (BudgetId, Name, Quantity, Cost, Price) values (" + budgetId + ", \"" + escape (Name) + "\", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
    public override void CreateForInvoice (int invoiceId) {
      new OleDbCommand ("insert into InvoiceArbitraries (InvoiceId, Name, Quantity, Cost, Price) values (" + invoiceId + ", \"" + escape (Name) + "\", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
    public override void CreateForTemplate (int templateId) {
      new OleDbCommand ("insert into TemplateArbitraries (TemplateId, Name, Quantity, Cost, Price) values (" + templateId + ", \"" + escape (Name) + "\", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
  }
  public class TemplateConcept : Concept {
    public string Description;
    public TemplateConcept (int id, string name, string description, double quantity, double cost, double price) : base (id, name, quantity, cost, price) {
      Description = description;
    }
    public TemplateConcept (OleDbDataReader dataReader) : base (dataReader) {
      Description = dataReader.GetString (5);
    }
    public static List<TemplateConcept> GetForBudget (int budgetId) {
      List<TemplateConcept> list = new List<TemplateConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select BudgetTemplates.TemplateId, Templates.Name, BudgetTemplates.Quantity, BudgetTemplates.Cost, BudgetTemplates.Price, BudgetTemplates.Description from BudgetTemplates inner join Templates on BudgetTemplates.TemplateId = Templates.Id where BudgetTemplates.BudgetId = " + budgetId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new TemplateConcept (dataReader));
      return list;
    }
    public override void CreateForBudget (int budgetId) {
      new OleDbCommand ("insert into BudgetTemplates (BudgetId, TemplateId, Quantity, Cost, Price, Description) values (" + budgetId + ", " + Id + ", \"" + Quantity + "\", \"" + Cost + "\", \"" + Price + "\", \"" + escape (Description) + "\")", connection).ExecuteNonQuery ();
    }
  }
  public class BudgetConcept : Concept {
    public BudgetConcept (int id, string name, double quantity, double cost, double price) : base (id, name, quantity, cost, price) { }
    public BudgetConcept (OleDbDataReader dataReader) : base (dataReader) { }
    public static List<BudgetConcept> GetForInvoice (int invoiceId) {
      List<BudgetConcept> list = new List<BudgetConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select InvoiceBudgets.BudgetId, Budgets.Number & \"\", InvoiceBudgets.Quantity, InvoiceBudgets.Cost, InvoiceBudgets.Price from InvoiceBudgets inner join Budgets on InvoiceBudgets.BudgetId = Budgets.Id where InvoiceBudgets.InvoiceId = " + invoiceId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new BudgetConcept (dataReader));
      return list;
    }
    public override void CreateForInvoice (int invoiceId) {
      new OleDbCommand ("insert into InvoiceBudgets (InvoiceId, BudgetId, Quantity, Cost, Price) values (" + invoiceId + ", " + Id + ", 1, \"" + Cost + "\", \"" + Price + "\")", connection).ExecuteNonQuery ();
    }
  }
  public class ReceiptConcept : Concept {
    public int Number;
    public DateTime DueDate;
    public DateTime? PayDate;
    public int? AccountEntryId;
    public int? AccountId;
    public string AccountName;
    public ReceiptConcept (int number, double price, DateTime dueDate, DateTime? payDate, int? accountId, string accountName): base (0, number.ToString (), 0, 0, price) {
      Number = number;
      DueDate = dueDate;
      PayDate = payDate;
      AccountId = accountId;
      AccountName = accountName;
    }
    public ReceiptConcept (OleDbDataReader dataReader) : base (dataReader) {
      Number = dataReader.GetInt32 (5);
      DueDate = dataReader.GetDateTime (6);
      PayDate = dataReader.IsDBNull (7) ? (DateTime?) null : dataReader.GetDateTime (7);
      AccountEntryId = dataReader.IsDBNull (8) ? (int?) null : dataReader.GetInt32 (8);
      AccountId = dataReader.IsDBNull (9) ? (int?) null : dataReader.GetInt32 (9);
      AccountName = dataReader.IsDBNull (10) ? null : dataReader.GetString (10) + " - " + dataReader.GetString (11);
    }
    public static List<ReceiptConcept> GetForInvoice (int invoiceId) {
      List<ReceiptConcept> list = new List<ReceiptConcept> ();
      OleDbDataReader dataReader = new OleDbCommand ("select 0, InvoiceReceipts.Number & \"\", cdbl(0), cdbl(0), InvoiceReceipts.Price, InvoiceReceipts.Number, InvoiceReceipts.DueDate, InvoiceReceipts.PayDate, InvoiceReceipts.AccountEntryId, AccountEntries.AccountId, Accounts.Code, Accounts.Name from (InvoiceReceipts left join AccountEntries on InvoiceReceipts.AccountEntryId = AccountEntries.Id) left join Accounts on AccountEntries.AccountId = Accounts.Id where InvoiceReceipts.InvoiceId = " + invoiceId, connection).ExecuteReader ();
      while (dataReader.Read ()) list.Add (new ReceiptConcept (dataReader));
      return list;
    }
    public override void CreateForInvoice (int invoiceId) {
      new OleDbCommand ("insert into InvoiceReceipts (InvoiceId, Price, [Number], DueDate, PayDate, AccountEntryId) values (" + invoiceId + ", \"" + Price + "\", " + Number + ", \"" + Data.Date_Format (DueDate) + "\", " + (PayDate == null ? "null" : "\"" + Data.Date_Format ((DateTime) PayDate) + "\"") + ", " + (AccountEntryId == null ? "null" : AccountEntryId.ToString ()) + ")", connection).ExecuteNonQuery ();
    }
  }
}

