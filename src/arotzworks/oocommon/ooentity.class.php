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
    
    $Id: ooentity.class.php,v 1.5 2006/01/27 16:55:24 kilburn Exp $
*/    
		class ooentity extends oobject 
		{
			var $__validators;
			var $__invalid_data = "Invalid Data";

			function validator($field,$type)
			{
				$this->__validators[$field][] = $type;
				return true;
			}	

			function validate($errors="")
			{
                                $this->__invalid_data = getClassConfig(new ooentity(),"invalid_data");
				
				foreach ($this as $k => $v)
				{
					if (strpos("__",$k) === false)
					{
						for($i=0;$i<count($this->__validators[$k]);$i++)
						{
							$function = $this->__validators[$k][$i];
							if ($function == "is_checked")
							{
								if ($v == "on")
								{
									$this->$k = 1;	
								} 
					
								if ($v == "off")
								{
									$this->$k = 0;	
								}
							}
							
							debug ($this,"Validating ".$k." with ".$function." for value ".$v);
							
							if ($function($v)===false)
							{
								debug ($this,"<font color=\"red\">Invalid field ".$k ." with value ".$v."</font>");
								$errors[] = $k;
								$e = new errorobject($this,1,$this->__invalid_data,$k);	
							} else {
								debug ($this,"<font color=\"green\">Valid field ".$k ." with value ".$v."</font>");
							}
						}	
					}
				}
				if (count($errors) > 0)
					return false;
				return true;	
			}

			
					
		}

	function is_checked(&$value)
	{
		if ($value != 1  && $value != 0)
			return false;
		return true;
	}

	
	$c = new ooentity();
	debug($c,"Loading Entities...");
	$f = new filesystem(getcwd().DIRECTORY_SEPARATOR."oocommon".DIRECTORY_SEPARATOR."entities");
	// Este include_once se podrá eliminar cuando los ficheros de clases ya incorporen sus
	// propios include_once a la clase padre
	include_once(getcwd()."/oocommon/entities/scaffold.class.php");
	for ($i = 0; $i < count($f->dir); $i++) {
		if (  $f->dir[$i]["filename"] != "scaffold" // ?
		   && file_exists($f->pwd.$f->dir[$i]["filename"]) 
		   && $f->dir[$i]["type"] == "FILE"  ) 
		{
			include_once ($f->pwd.$f->dir[$i]["filename"]);
		}
	}
	

?>
