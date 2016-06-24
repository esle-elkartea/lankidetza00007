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
    
    $Id: ooutils.php,v 1.6 2006/01/27 16:55:24 kilburn Exp $
*/    
	$__oocommon_version = "0.79.9";
	$__oocommon_packager = "luis@webalianza.com";
	
	
	$__debug_array = array();

	
	function includeall($dir)
	{
		$path = getcwd(). DIRECTORY_SEPARATOR . $dir . DIRECTORY_SEPARATOR;
		$f = new filesystem( $path );
		for ($i = 0; $i < count($f->dir); $i ++) {
			if (  file_exists( $path . $f->dir[$i]["filename"] ) // Realmente hace falta?
			   && $f->dir[$i]["type"] == "FILE")
			{
				//$mem = memory_get_usage();
				include_once ( $path . $f->dir[$i]["filename"] );
			}
		}
	}
	
	function getUrl()
	{
		$base_url =  $_SERVER["PHP_SELF"];
		$r = new request();
		$base_url .= "?__baseurl=0";
		foreach ($r as $k=>$v)
		{	
			if (strpos($k,"__") === false ) $base_url .="&".$k."=".$v;
		}
		$base_url .="&id=".$r->id;
		$base_url .="&sid=".$r->sid;
		$base_url .="&tid=".$r->tid;
		
		return $base_url;
	}	

        function getUrlBut($but)
        {
                $base_url =  $_SERVER["PHP_SELF"];
                $r = new request();
                $base_url .= "?__baseurl=0";
                foreach ($r as $k=>$v)
                {       
                        if (strpos($k,"__") === false && strpos($k,$but) === false ) $base_url .="&".$k."=".$v;
                }
                $base_url .="&id=".$r->id;
                $base_url .="&sid=".$r->sid;
                $base_url .="&tid=".$r->tid;
                
                return $base_url;
        }       
	
	function microtime_float()
	{
   	list($usec, $sec) = explode(" ", microtime());
   	return ((float)$usec + (float)$sec);
	}

	$__utime_start = microtime_float();
	
        function not_null($a)
        {
                if ($a != "") return true;
                return false;
        }

	function debug($o,$message)
	{
		global $__debug_array;
		
		$a["object"] =  (method_exists($o,"toString") ? $o->toString() : "scratch");
		$a["message"] = $message;
		$__debug_array[] = $a;
		
	}

	function check_sign($o)
	{
		if ($o > 0 && is_numeric($o)) return true;
		return false;	
	}

	function is_set($o)
	{
		return isset($o);	
		
	}

	function is_email($var)
	{
		if ($var == null) return true;
		$atom = '[-a-z0-9!#$%&\'*+/=?^_`{|}~]';   
		$domain = '([a-z]([-a-z0-9]*[a-z0-9]+)?)';
		$regex = '^' . $atom . '+' .        
		'(\.' . $atom . '+)*'.              
		'@'.                                
		'(' . $domain . '{1,63}\.)+'.       
		$domain . '{2,63}'.                 
		'$';                                
		return eregi($regex, $var);
	}
	
	function is_url($var)
	{
		
	}

	function ooerror($number,$message)
	{
		user_error($message);
		
		$d = debugWidget::widget();
		echo $d->draw();
		
	}
	
	function print_arr($arr) 
	{
	echo "<table class=\"print_arr\">";
	foreach ($arr as $k => $v) {
		echo "<tr>
				<td class=\"print_arr_field\">
				".$k."</td>
				<td class=\"print_arr_value\">
				";
		if (is_array($v) || is_object($v)) {
			print_arr($v);
		} else {
			echo "<b>".$v."</b>";
		}
		echo "
				</td>
				</tr>";
	}
	echo "</table>";
	}
	
	function ret_arr($arr) 
	{
	$out .= "<table class=\"print_arr\">";
	foreach ($arr as $k => $v) {
		$out .= "<tr>
				<td class=\"print_arr_field\">
				".$k."</td>
				<td class=\"print_arr_value\">
				";
		if (is_array($v) || is_object($v)) {
			$out .=ret_arr($v);
		} else {
			$out .= "<b>".$v."</b>";
		}
		$out .= "
				</td>
				</tr>";
	}
	$out .= "</table>";
	return $out;
	}
	
	
	
class filesystem {
	var $pwd;
	var $dir;
	// para qué sirve?
	var $files;
	// y esta?
	var $last_filehandle;
	function toString() { return "filesystem"; }
	
	function filesystem($dirname = DIRECTORY_SEPARATOR) {
		if (!is_dir($dirname))
			return;

		if ( substr($dirname,-1,1) != DIRECTORY_SEPARATOR )
			$dirname .= DIRECTORY_SEPARATOR;
			
		$this->pwd = $dirname;
		$handle = opendir($dirname);
		$i=0;
		while (false !== ($file = readdir($handle))) {
                               
			if ($file == '.' || $file == '..')
				continue;
			
			$this->dir[$i]['filename'] = $file;
			$this->dir[$i]['type']     = is_dir($dirname.$file) ? 'DIRECTORY' : 'FILE';
			$i++;
		}
		closedir($handle);
	}
}