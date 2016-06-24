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
    
    $Id: progressbar.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class progressbar extends oowidget
	{
		var $progress;
		var $max = 100;	
		var $ratio = 1;
		var $show_number=true;
		var $css_container = "progress_container";
		var $css_bar = "progress_bar";
		var $css_ammount = "progress_ammount";
				
		function onReceive()
		{
				$o = $this->popMailBox();
				if ($o->progress) { $this->progress = $o->progress; }	
		}			
				
		function draw()
		{
			if ($this->max != 100 && $this->ratio == 1.0)
			{
				$this->ratio = $this->max / 100;	
			}
			$width = round($this->progress / $this->ratio);
			$cwidth = round ($this->max / $this->ratio);
			
			if ($width > $cwidth) $width = 100;
						
			$out .="
			<div class=\"".$this->css_container."\" style=\"width:".$cwidth.";float:left\">
			<div class=\"".$this->css_bar."\" style=\"width:".$width."\">
			&nbsp;
			</div>	
			</div>
			";
			if ($this->show_number)
				$out .= "&nbsp;".$width. "%";
			return $out;
		}
		
		
		
	}