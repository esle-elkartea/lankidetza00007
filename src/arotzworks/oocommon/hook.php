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
    
    $Id: hook.php,v 1.5 2006/01/03 20:49:51 luis Exp $
*/    
	$hook_status = true;
	$hook = array();
	$globalhook = array();
	class hook {
	
		function addListener($hookname,$class,$method="")
		{
			if (!$method) $method = $hookname;
			global $hook;
			assert($hookname);
			assert($class);
			$temp["class"] = $class;
			$temp["method"] = $method;
			$hook[$hookname][] = $temp;
			//debug ($this,"<font color=\"magenta\">Registered Hook Handler <b>".$class."->".$method."()</b> for hook <b>".$hookname."</b></font>");
		}
		
		function disable()
		{
			global $hook_status;
			$hook_status = false;
		
		}
		
		function enable()
		{
			global $hook_status;
			$hook_status = true;
		
		}
		
		function addGlobalListener($class,$method)
		{
			global $globalhook;
			assert ($class);
			assert ($method);
			$temp ["class"] = $class;
			$temp ["method"] = $method;
			$globalhook[] = $temp;
		}
		
		function allowHook($hook,$class)
		{
				// Esto es necesario para el modulo SDK
			$r = new request();
			$e = $r->__osl_sdk;
			$e = str_replace ("\\","",$e);
			$os = unserialize($e);
			$honame = $hook."_".$class;
			if (is_array($os->hooks))
				return ($os->hooks[$honame]);
			return true;
		}
		
		function call ($hookname,$param)
		{
			global $hook_status;
			if (!$hook_status ) return;
			global $hook;
			global $globalhook;
			
			debug ($this,"<b>Calling hook ".$hookname." with parameter ".$param->toString()."</b>");
			if (is_array($hook[$hookname]))
			{
				$ho = $hook[$hookname];
				for($i=0;$i<count($ho);$i++)
				{
					if (hook::allowHook($hookname,$ho[$i]["class"]))
					{
						debug ($this,"<font color=\"magenta\">Calling hook listener <b>".$ho[$i]["class"]."->".$ho[$i]["method"]."()</b></font>");
						assert($param);
						$o = new $ho[$i]["class"];
						$o->drop($param);
						$method = $ho[$i]["method"];
						$o->$method();
					} else {
						debug ($this,"<b style=\"color:rgb(255,0,0);\">Calling Denied for hook ".$hookname." with parameter ".$param->toString()."</b>");		
					}
				}
			}
			for($i=0;$i<count($globalhook);$i++)
			{
				$o = new $globalhook[$i]["class"];
				$o->drop($param);
				$method = $globalhook[$i]["method"];
				$o->$method($hookname);
			}
		}
	}
?>