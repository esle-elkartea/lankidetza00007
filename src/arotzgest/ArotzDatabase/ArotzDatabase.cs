/*  ArotzDatabase
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programación: Jorge Moreiro
 *                Iker Estébanez
 * 
 *  Este fichero forma parte de ArotzDatabase
 *
 * ArotzDatabase es software libre; puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versión 2 de la licencia o
 * (según su elección) cualquier versión posterior.
 * 
 * ArotzDatabase es redistribuido con la intención que sea útil, pero SIN NINGUNA
 * GARANTÍA, ni tan solo las garantías implícitas de MERCANTABILIDA o ADECUACIÓN 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para más detalles.
 * 
 * Debería haber recibido una copia de la GNU General Public License acompañando a 
 * ArotzDatabase.
 * 
 * ÉSTE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - más información en http://www.spri.es
 * 
 *
 * */

using ADOX;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

class ArotzDatabase {
  static string name = "ArotzDatabase";
  static string version = "1.0";
  static string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "ArotzGest.mdb";
  static string connectionString = "Provider = Microsoft.Jet.OleDb.4.0; Data Source = " + fileName;
  static void Main (string [] args) {
    if (File.Exists (fileName)) {
      if (MessageBox.Show ("La base de datos ya existe, desea regenerarla?", name, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
      File.Delete (fileName);
    }
    new CatalogClass ().Create (connectionString);
    OleDbConnection connection = new OleDbConnection (connectionString);
    connection.Open ();
    new OleDbCommand ("create table Accounts (Id identity primary key, Name varchar (255) not null, Code varchar (255) not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table AccountEntries (Id identity primary key, Name varchar (255) not null, AccountId int not null, [Date] date not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Budgets (Id identity primary key, [Number] int not null, ClientId int not null, [Date] date not null, State int not null, WarehouseId int)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table BudgetArbitraries (BudgetId int not null, Name varchar (255) not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table BudgetEmployeeCategories (BudgetId int not null, EmployeeCategoryId int not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table BudgetTemplates (BudgetId int not null, TemplateId int not null, Quantity double not null, Cost double not null, Price double not null, Description text not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table BudgetReports (BudgetId int not null, Type int not null, EmployeeCategoryId int not null, EmployeeId int, EstimatedQuantity double not null, Quantity double)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Clients (Id identity primary key, Name varchar (255) not null, FIC varchar (255) not null, Address varchar (255) not null, City varchar (255) not null, Province varchar (255) not null, ZIP varchar (255) not null, Phone varchar (255) not null, Mobile varchar (255) not null, Email varchar (255) not null, Homepage varchar (255) not null, Account varchar (255) not null, Notes text not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Company (Name varchar (255) not null, FIC varchar (255) not null, Address varchar (255) not null, City varchar (255) not null, Province varchar (255) not null, ZIP varchar (255) not null, BudgetValidDays int not null, LogoType int not null, LogoImage varchar (255) not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Employees (Id identity primary key, Name varchar (255) not null, CategoryId int not null, FIN varchar (255) not null, SSN varchar (255) not null, Address varchar (255) not null, City varchar (255) not null, Province varchar (255) not null, ZIP varchar (255) not null, Phone varchar (255) not null, Mobile varchar (255) not null, Email varchar (255) not null, Account varchar (255) not null, Notes text not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table EmployeeCategories (Id identity primary key, Name varchar (255) not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Invoices (Id identity primary key, [Number] int not null, ClientId int not null, [Date] date not null, VAT int not null, Retention int not null, State int not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table InvoiceArbitraries (InvoiceId int not null, Name varchar (255) not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table InvoiceBudgets (InvoiceId int not null, BudgetId int not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table InvoiceReceipts (InvoiceId int not null, [Number] int not null, Price double not null, DueDate date not null, PayDate date, AccountEntryId int)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Materials (Id identity primary key, Name varchar (255) not null, CategoryId int not null, ProviderId int not null, Type int not null, Dimension1 double not null, Dimension2 double not null, Dimension3 double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table MaterialCategories (Id identity primary key, Name varchar (255) not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Providers (Id identity primary key, Name varchar (255) not null, FIC varchar (255) not null, Address varchar (255) not null, City varchar (255) not null, Province varchar (255) not null, ZIP varchar (255) not null, Phone varchar (255) not null, Mobile varchar (255) not null, Email varchar (255) not null, Homepage varchar (255) not null, Account varchar (255) not null, Notes text not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Templates (Id identity primary key, Name varchar (255) not null, CategoryId int not null, Description text not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table TemplateArbitraries (TemplateId int not null, Name varchar (255) not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table TemplateCategories (Id identity primary key, Name varchar (255) not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table TemplateEmployeeCategories (TemplateId int not null, EmployeeCategoryId int not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table TemplateMaterials (TemplateId int not null, MaterialId int not null, Type int not null, Dimension1 double not null, Dimension2 double not null, Dimension3 double not null, Quantity double not null, Cost double not null, Price double not null)", connection).ExecuteNonQuery ();
    new OleDbCommand ("create table Warehouses (Id identity primary key, Name varchar (255) not null)", connection).ExecuteNonQuery ();
    MessageBox.Show ("La base de datos se ha generado con éxito", name, MessageBoxButtons.OK, MessageBoxIcon.Information);
  }
}

