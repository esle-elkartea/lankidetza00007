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
    
    $Id: datajoiner.class.php,v 1.3 2006/01/23 15:28:40 luis Exp $
*/    
include_once("dataobject.class.php");
	class datajoiner extends dataobject
	{
		var $foreigntable;
		var $localtable;
		var $lk;
		var $fk;
		
					
		
		function open($id)
		{
		
			$object1 = $this->popMailBox();
			$object2 = $this->popMailBox();
			$this->localtable = $object1->__table;
			$this->foreigntable = $object2->__table;
			
			

			foreach ($object1 as $k=>$v)
			{
				if ($k!="id" && strpos($k,"__") === false) $of1 .= $object1->__table.".".$k.",";				
			}
			
			foreach ($object2 as $k=>$v)
			{
				if ($k!="id" && strpos($k,"__") === false) $of1 .= $object2->__table.".".$k.",";				
			}
			
			

			$db = new database();
			$db->query ("SELECT ".$of1." ".$of2." ".$this->localtable.".id FROM ".$this->localtable." INNER JOIN ".$this->foreigntable." ON (".$this->localtable.".".$this->lk." = ".$this->foreigntable.".".$this->fk.") WHERE ".$this->localtable.".id=".$id); 
			foreach ($db->rows[0] as $k => $v) 
			{
				$this->$k = $v;
			}
			$this->id = $db->rows[0]["id"];
			
		}		
		
		function getList()
		{
						$object1 = $this->popMailBox();
			$object2 = $this->popMailBox();
			$this->localtable = $object1->__table;
			$this->foreigntable = $object2->__table;
			
			

			foreach ($object1 as $k=>$v)
			{
				if ($k!="id" && strpos($k,"__") === false) $of1 .= $object1->__table.".".$k.",";				
			}
			
			foreach ($object2 as $k=>$v)
			{
				if ($k!="id" && strpos($k,"__") === false) $of1 .= $object2->__table.".".$k.",";				
			}
			
			$db = new database();
			$db->query("SELECT ".$of1." ".$of2." ".$this->localtable.".id FROM ".$this->localtable." INNER JOIN ".$this->foreigntable." ON (".$this->localtable.".".$this->lk." = ".$this->foreigntable.".".$this->fk.")"); 
				
			return $db->rows;			
		}
		
		
		
	}


?>