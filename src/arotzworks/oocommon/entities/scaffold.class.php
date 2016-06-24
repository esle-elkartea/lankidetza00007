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
    
    $Id: scaffold.class.php,v 1.3 2006/01/19 09:07:00 luis Exp $
*/    
include_once("dataobject.class.php");
	class scaffold extends dataobject
	{
		function scaffold($table)
		{
			if (!$table) return null;
			$this->__table = $table;
			$db = new database();
			$db->query ("SHOW FIELDS FROM ".$table);	
			for ($i=0;$i<count($db->rows);$i++)
			{
				$fieldname = $db->rows[$i]["Field"];
				$this->$fieldname = false;
				$type =" ".$db->rows[$i]["Type"];
				if ($db->rows[$i]["Extra"] != "auto_increment")
					{
						if (strpos ($type,"int") != false)
						{
							if (strpos ($type,"tinyint(1)") != false)
							{
								$this->validator($fieldname,is_checked);
								$this->__datafields[$fieldname]="bool";
								
							} else {
								$this->validator($fieldname,is_numeric);
								$this->__datafields[$fieldname]="int";
							}
						}
						if (strpos ($type,"date") != false)
						{
							//$this->validator($fieldname,is_numeric);
							$this->__datafields[$fieldname]="date";
						}
						
						if (strpos ($type,"float") != false)
						{
							$this->validator($fieldname,is_numeric);
							$this->__datafields[$fieldname]="float";
						}
						if (strpos ($type,"unsigned") != false)
							$this->validator($fieldname,check_sign);
						if (strpos ($type,"text") != false){
							$this->__datafields[$fieldname]="text";}
						if (strpos ($type,"varchar") != false){
							$this->__datafields[$fieldname]="varchar";}
					}
			}
		}	
	}

?>