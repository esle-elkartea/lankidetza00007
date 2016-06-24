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
    
    $Id: oocommon.php,v 1.6 2006/01/03 09:00:48 luis Exp $
*/    
	include ("ooconfig.php");
	include ("ooutils.php");
	class oocommon {}
	debug (new oocommon(),"<b>Initial Infraestructure loaded</b>");
	include ("oobject.class.php");
	include ("hook.php");
	$o = new oobject();
	debug ($o,"oobject signature check ok");
	debug ($o,"<b>Webalianza ooCommon PHP Framework ".$__oocommon_version." - released by ".$__oocommon_packager."</b>");
	debug ($o,"<b>&copy; 2002-2003-2004-2005-2006 Webalianza T.I. S.L.</b>");
	debug ($o,"<b>Licensed under the GNU General Public License</b>");
	include ("oocomponent.class.php");
	include ("ooentity.class.php");
	include ("oowidget.class.php");
	

function handle_error($errno, $errstr, $errfile, $errline, $errcontext) {
	global $__oocommon_version;
	if ($errno == 8)
		return;

	$bt = debug_backtrace();
	for ($i = count($bt) - 1; $i > 1; $i --) {
		for ($k = 0; $k < count($error_stop_classes); $k ++) {
			if ($bt[$i]["class"] == $error_stop_classes[$k])
				$flag = 1;
		}
	}

	$bt = debug_backtrace();

	echo "
					<div style=\"float:left;font-size:328px;color:rgb(255,0,0);font-family:Serif	;\"><b>!</b></div>
					<div style=\"font-family:verdana;width:890px;border:outset 1px; 1px;padding:5px;\"><b style=\"letter-spacing:-4px;font-size:26px;color:rgb(255,70,10);font-family:Sans;\">
					Ooops! An error has ocurred!</b><br />
					 
			";

	echo "<span style=\"font-family:verdana;\"><b>Guru meditation error :</b> ".$errstr."<br /><br /></span>";
	echo "<table border=\"1\" style=\"background:rgb(245,245,254);color:Rgb(25,25,25)\" cellspacing=\"3\"><tr><td>Current URL</td><td> ".getUrl()."</td></tr>";
	$r = new request();
	echo "<tr><td >Request Variables</td><td>".ret_arr($r)."</td></tr>";
	for ($i = count($bt) - 1; $i > 0; $i --) {
		for ($k = 0; $k < count($error_stop_classes); $k ++) {
			if ($bt[$i]["class"] == $error_stop_classes[$k])
				$flag = 1;
		}
		if ($bt[$i]["function"] != "handle_error" && $bt[$i]["function"] != "user_error" )
		{
			if ($first == false)
			{
			echo "<tr><td colspan=\"2\"><span style=\"font-size:14px;font-weight:bold;\">Originated at ".$bt[$i]["file"].", line <b>".$bt[$i]["line"]."</b>,  <u>".($bt[$i]["class"] ? $bt[$i]["class"]."::" : "").$bt[$i]["function"]."()</u></span></td></tr>
";
			$first = true;
			} else {
				echo "<tr><td>Invoqued</td><td> ".$bt[$i]["file"].", line <b>".$bt[$i]["line"]."</b>,  <u>".($bt[$i]["class"] ? $bt[$i]["class"]."::" : "").$bt[$i]["function"]."()</u></td></tr>
";
			}
		} else {
			if (strpos($bt[$i]["function"],"error") != false)
			{
				echo "<tr><td colspan=\"2\" style=\"text-align:center;background:rgb(215,0,0);color:rgb(255,255,255);\"><b>Error Raised Here</b></td></tr>";	
			}	
			
		}
	}
	echo "</table><br />";
	echo "<hr>";
	echo "<b>Webalianza ooCommon ".$__oocommon_version." </b></div>";
	die();
}
	
	set_error_handler("handle_error");
