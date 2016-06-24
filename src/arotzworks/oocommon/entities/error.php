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
    
    $Id: error.php,v 1.3 2005/12/27 16:25:15 luis Exp $
*/    
	$__gl_errors = array();
	class errorobject extends ooentity
	{
		var $object;
		var $errno;
		var $string;
		var $param;
		function errorobject($object,$errno,$string,$param)
		{
			$this->object = $object;
			$this->errno = $errno;
			$this->param = $param;
			$db = new database();
			$db->query ("SELECT string_locale FROM oocommon_errors WHERE errno=".$errno." AND object='".get_class($object)."'");
			$this->string = ($db->rows[0]["string_locale"] ? $db->rows[0]["string_locale"] : $string);		
			global $__gl_errors;
			$__gl_errors[] = $this;
		}	
		
		function errorParams()
		{
			global $__gl_errors;
			for ($i=0;$i<count($__gl_errors);$i++)
			{
				if ($__gl_errors[$i]->param) $params[$__gl_errors[$i]->param] = true;;
			}	
			return $params;
		}
		
		
                function addError($string)
                {
                   global $__gl_errors;
                        $__gl_errors[]->string = $string;

                }

                function hasErrors()
                {
                   global $__gl_errors;
                   if (count($__gl_errors) == 0) return false;
                   return true;     
                }

		function errorList()
		{
		
			global $__gl_errors;
		
			for ($i=0;$i<count($__gl_errors);$i++)
			{
				$out .="<li>".$__gl_errors[$i]->string.($__gl_errors[$i]->param ? " : ".$__gl_errors[$i]->param : "" )."</li>";				
			}	
			return html::write($out);
		}
		
		function __install()
		{
			
			$db = new database();
			$db->query("
			CREATE TABLE oocommon_errors (
			id int(10) unsigned not null auto_increment,
			object varchar(40),
			errno int(10),
			locale varchar(40),
			string_original text,
			string_locale text,
			PRIMARY KEY(id));
			");
		}
	}