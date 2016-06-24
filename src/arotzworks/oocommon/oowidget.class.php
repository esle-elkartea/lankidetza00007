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
    
    $Id: oowidget.class.php,v 1.3 2006/01/27 16:55:24 kilburn Exp $
*/    

	class oowidget extends oobject 
	{
		var $__data;
		var $__widgetfields;
		var $width="100%";
		var $display;
		var $__mailbox;
						
			function addwidget($o,$field="BODY")
			{
                                assert($o);
				debug ($this,"<font color=\"green\">Adding widget ".$o->toString()." to rendering pipeline</font>");
				if ($field == "BODY") $field = count($this->__widgetfields);
				$this->__widgetfields[$field] = $o;		
			}
							
			function drawWidget()
			{
				return $this->drawContainedWidgets();
			}

			function drawContainedWidgets()
			{
				
				foreach ($this->__widgetfields as $k=>$v)
				{
					$out .= $v->draw();		
				}
				return $out;
			}

			function draw()
			{
				debug ($this,"<font color=\"green\"><b>Drawing widget</b></font>");
                                $out .="\n<div class=\"".get_class($this)."_widget\" style=\"width:".$this->width.";height:".$this->height."\">";
				$out .=$this->drawWidget();
				$out .="</div>\n";	
				return $out;	
			}
	}
	
	$c = new oowidget();
	debug($c,"Loading Widgets...");
	$f = new filesystem(getcwd().DIRECTORY_SEPARATOR."oocommon".DIRECTORY_SEPARATOR."widgets");
	for ($i = 0; $i < count($f->dir); $i ++) {
	if (  file_exists($f->pwd.$f->dir[$i]["filename"]) 
	   && $f->dir[$i]["type"] == "FILE")
	{
		//$mem = memory_get_usage();
		include_once ($f->pwd.$f->dir[$i]["filename"]);
		//debug (new oowidget(), "".$f->dir[$i]["filename"]." using ".(memory_get_usage()-$mem)." Bytes");
	}
	}
	