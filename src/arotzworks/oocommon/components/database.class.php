<?php
/*
Webalianza ooCommon NG PHP Framework
(c) 2005 Webalianza T.I. S.L.
Development sponsored and started by
Webalianza T.I. S.L.
Project Lead:   Luis Martin-Santos Garcia <luis@webalianza.com>

This file is part of ooCommon NG.

    ooCommon NG is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    ooCommon is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with ooCommon NG; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
    
    $Id: database.class.php,v 1.6 2006/02/01 15:22:42 luis Exp $
*/    
class database extends oocomponent {
	var $rows;
	var $mysql_username;
	var $mysql_password;
	var $mysql_hostname;
	var $mysql_database;
	var $rowlimit;
	var $table;
	var $link;
	function database() {
		$this->accept("dataobject");
		$this->mysql_username = getClassConfig($this, "mysql_username");
		$this->mysql_password = getClassConfig($this, "mysql_password");
		$this->mysql_hostname = getClassConfig($this, "mysql_hostname");
		$this->mysql_database = getClassConfig($this, "mysql_database");
		$this->rowlimit = getClassConfig($this, "rowlimit");
		$this->link = mysql_pconnect($this->mysql_hostname, $this->mysql_username, $this->mysql_password);
		if (!$this->link)
			ooerror(E_CODEERROR, "Could not connect to MySQL Database");
		mysql_select_db($this->mysql_database, $this->link);
		mysql_select_db($database, $this->link);
		
	}

	function query($sql) {
		$ts = microtime();
		$this->rows = array();
		$result = mysql_query($sql, $this->link);
		if (mysql_error() != "") {
			ooerror(E_CODEERROR, "MySQL: ".mysql_error()."<br /> Query:".$sql);
		}
		$n = 0;
		if (strstr($sql, "UPDATE") == FALSE && strstr($sql, "DELETE") == FALSE && strstr($sql, "INSERT") == FALSE && strstr($sql, "CREATE") == FALSE && strstr($sql, "DROP") == FALSE && strstr($sql, "ALTER") == FALSE) {
			if ($result)
				while ($temprow = mysql_fetch_assoc($result)) {
					foreach ($temprow as $k => $v) {
						$this->rows[$n][$k] = $v;
					}
					$n ++;
				}
		}
		debug($this, "query:<b>".substr($sql,0,128)."...</b>, ".(microtime()-$ts). " seconds");
	}
	
	function quickQuery($q,$f)
	{	
		$db = new database();
		$db->query ($q);
		return $db->rows[0][$f];
	}

	function openTable()
	{
		if ($this->table)
		$this->query ("SELECT * FROM ".$this->table);	
	}


	function insert() 
	{
		$object[0] = $this->popMailBox();
		if ($this->table == "") {
			return false;
		}
		foreach ($object as $a) {
			$n ++;
			foreach ($a as $k => $v) {
				
				if (strpos($k,"__")===false)
				{
					$strs = $strs."\"".$v."\",";
					$strd = $strd.$k.", ";
				}
			}
			$strs = substr($strs, 0, -1);
			$strd = substr($strd, 0, -2);
			$que[$n] = "INSERT INTO ".$this->table." (".$strd.") VALUES ( ".$strs." )";
			$this->query($que[$n], $this->table);
		}
		if (!mysql_error())return true;
	}
	
	function onReceive()
	{
		debug ($this,"Preparing received object...");	
	}

	function update() 
	{
		$object = $this->popMailBox();
		foreach ($object as $k => $v) 
		{
			if (strpos($k,"__") === false)
			{
				$strs = $strs.$k." = \"".$v."\", ";
			}
		}
		$strs = substr($strs, 0, -2);
		$que = "UPDATE ".$this->table." SET ".$strs." WHERE id=".$object->id;
		$this->query($que);
		if (!mysql_error())return true;
	}

	function delete() {
		$object = $this->popMailBox();
		$this->query("DELETE FROM ".$this->table." WHERE id=".$object->id);
		if (!mysql_error())return true;
	}

	function getDataObject() {

	}
}
?>
