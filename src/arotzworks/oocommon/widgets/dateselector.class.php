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
    
    $Id: dateselector.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class dateselector extends oowidget
	{
		var $__prefix;
		var $selector_day = "day";
		var $selector_month = "month";
		var $selector_year = "year";
		var $selector_hour = "hour";
		var $selector_minutes = "minutes";
		
		function dateselector($prefix = "", $timestamp ="")
		{	
			
		$this->__prefix = $prefix;	
		if ($timestamp)
		{
			$d = new date();
			$d->timestamp = $timestamp;
			$this->drop($d);
		}
		}
		
		function draw()
		{
			$object = $this->popMailbox();
			if (!$object) { $object = new date(); $object->timestamp = time(); $object->parse(); }
			//if (!$object) $object = new date(); $object->timestamp = 0; $object->parse();
			
			$out .="<select name=\"".($this->__prefix ? $this->__prefix."_" : "") .$this->selector_day."\">";
			$out .="<option value=\"0\" ".($object->string=="N/A" ? "selected" : "").">--</option>";
			for ($i=1;$i<32;$i++)
			{
				$out .="\n\t<option value=\"".$i."\" ".($object->day == $i ? "selected" : "").">".$i."</option>";
			}
			$out .="</select>";
			$out .="<select name=\"".($this->__prefix ? $this->__prefix."_" : "") .$this->selector_month."\">";
			$out .="<option value=\"0\" ".($object->string=="N/A" ? "selected" : "").">--</option>";
			for ($i=1;$i<13;$i++)
			{
				$out .="\n\t<option value=\"".$i."\" ".($object->month == $i ? "selected" : "").">".$object->__months[$i]."</option>";	
				
			}	
			$out .="</select>";	
			$out .="<select name=\"".($this->__prefix ? $this->__prefix."_" : "") .$this->selector_year."\">";
			$out .="<option value=\"0\" ".($object->string=="N/A" ? "selected" : "").">--</option>";
			for ($i=1970;$i<2020;$i++)
			{
				$out .="\n\t<option value=\"".$i."\" ".($object->year == $i ? "selected" : "").">".$i."</option>";	
				
			}	
			$out .="</select>";	
			//$out .="<input type=\"text\" name=\"".($this->__prefix ? $this->__prefix."_" : "").$this->selector_hour."\" value=\"".$object->hour."\" size=\"2\"> : <input type=\"text\" name=\"".($this->__prefix ? $this->__prefix."_" : "").$this->selector_minutes."\" value=\"".$object->minutes."\" size=\"2\">";
			
			return $out;
			
		}	
		
		
		
		
		
		
		
	}





