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
    
    $Id: oocomponent.class.php,v 1.3 2006/01/27 16:55:24 kilburn Exp $
*/    

	class oocomponent extends oobject
	{
		var $__accepts;
		var $__mailbox;
			
		function accept($class)
		{
			$this->__accepts[] = $class;	
		}
		
	}
	
		
	$c = new oocomponent();
	debug($c,"Loading Components...");
	$f = new filesystem(getcwd().DIRECTORY_SEPARATOR."oocommon".DIRECTORY_SEPARATOR."components");
	for ($i = 0; $i < count($f->dir); $i ++) {
		if (  file_exists($f->pwd.$f->dir[$i]["filename"]) 
		   && $f->dir[$i]["type"] == "FILE") 
		{
			include_once ($f->pwd.$f->dir[$i]["filename"]);
		}
	}
	
