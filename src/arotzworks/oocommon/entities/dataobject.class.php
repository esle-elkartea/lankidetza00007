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
    
    $Id: dataobject.class.php,v 1.5 2006/01/31 08:09:11 luis Exp $
*/    
	class dataobject extends ooentity {
		
		var $__table;
		var $__query;		
		function insert()
		{
			$this->onInsert();
			if (!$this->validate($errors)) return false;
			$db = new database();
			$db->drop($this);
			$db->table = $this->__table;
			return $db->insert();
		}
		
		function setCondition($field,$value)
		{
			$this->__query = "SELECT * FROM ".$this->__table." WHERE ".$field." = '".$value."'";
		}
		
		function getFields()
		{
			$db = new database();
			$db->query ("SHOW FIELDS FROM ".$this->__table);	
			for ($i=0;$i<count($db->rows);$i++)
			{
				$fieldname[] = $db->rows[$i]["Field"];
			}
			return $fieldname;
		}
		
		function update()
		{
			$this->onUpdate();
			if (!$this->validate($errors)) return false;
			$db = new database();
			$db->table = $this->__table;
			$db->drop($this);
			return $db->update();	
		}
		
		function delete ()
		{
			$this->onDelete();
			if (!$this->validate($errors)) return false;
			$db = new database();
			$db->table = $this->__table;
			$db->drop($this);
			return $db->delete();
		}
		
		function composeField($field,$entity,$efield)
		{
			$obj = new $entity();			
			debug($this,"<font color=\"magenta\">Composition for <b>".$field."</b> with entity <b>".$entity."</b> (foreign field <b>".$efield."</b>)</font>");
			$obj->fromObject(new request());
			$obj->drop(new request());
			$this->$field = $obj->$efield;
		}

		function getList()
		{
			$this->onGetList();
			$db = new database();
			if ($this->__query != "")
			{
				$db->query ($this->__query);	
			} else {
			$db->table = $this->__table;
			$db->openTable();
			}
			return $db->rows;			
		}
		
		function openByQuery($q)
		{
			$db = new database();
			$db->table = $this->__table;
			$db->query ($q);			
			foreach ($this as $k => $v) 
			{	
				if (strpos($k,"__") === false)
					$this->$k = $db->rows[0][$k];
			}
			$this->id = $db->rows[0]["id"];
			$this->onOpen();
		}
		
		function open($id)
		{
			$db = new database();
			$db->table = $this->__table;
			$db->query ("SELECT * FROM ".$this->__table." WHERE id=".$id);			
			foreach ($this as $k => $v) 
			{	
				if (strpos($k,"__") === false)
					$this->$k = $db->rows[0][$k];
			}
			
			$this->id = $db->rows[0]["id"];
			$this->onOpen();
		}

		function GetByField($field,$value)
		{
			$this->setCondition($field,$value);
			$list = $this->getList();
			if (count($list)>0)
			{
				$this->open($list[0]["id"]);
				return true;
			} else {
				return false;
			}
		}

		
		function onInsert(){}
		function onUpdate(){}
		function onDelete(){}
		function onOpen(){}
		function onGetList(){}
	}
?>
