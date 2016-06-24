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
    
    $Id: date.class.php,v 1.4 2006/01/23 15:28:40 luis Exp $
*/    
	class date extends ooentity
	{
		var $year;
		var $month;
		var $day;
		var $hour;
		var $minutes;
		var $seconds;
		var $weekday;
		var $strweekday;
		var $strmonth;
		var $string;
                var $short_string;
		var $timestamp;	
		var $locale;
		var $__weekdays;
		var $__months;
								
		function date($timestamp ="")
		{
			if ($timestamp) {
				$this->now = $timestamp;
			} else {
				$this->now = time();
			}
			//$this->timestamp = $this->now;
			$this->locale = getClassConfig($this,"locale");	
		}

		function parse()
		{
			setlocale(LC_TIME, $this->locale);
			if ($this->day && $this->month && $this->year)
			{
				$this->timestamp = mktime($this->hour,$this->minutes,$this->seconds,$this->month,$this->day,$this->year);
			}
			for ($i = 0; $i < 7; $i ++) {
				$this->__weekdays[$i] = ucfirst(strftime("%a", $i * 86400 + (3 * 86400)));
			}
			for ($i = 1; $i < 13; $i ++) {
				
				$this->__months[$i] = ucfirst(strftime("%B", mktime(0,0,0,$i,1,$this->year)));
			}
			
			list ($this->weekday, $this->day, $this->month, $this->year, $this->hour, $this->minutes, $this->seconds) = explode(" ", date("w j n Y G i s", $this->timestamp));
			$this->strmonth = $this->__months[$this->month];
			$this->strweekday = $this->__weekdays[$this->weekday];
			//$this->string = $this->__weekdays[$this->weekday]." ".$this->day." ".$this->__months[$this->month]." ".$this->year." ".$this->hour.":".$this->minutes;	
			$this->string = $this->day."/".$this->month."/".$this->year." ".$this->hour.":".$this->minutes;	
                        $this->short_string = substr($this->__weekdays[$this->weekday],0,4)." ".$this->day." ".substr($this->__months[$this->month],0,3)." ".$this->year;  
			debug($this,"<font color=\"magenta\"> Parsed Date, timestamp = ".$this->timestamp." , string = ".$this->string); 			
			if ($this->timestamp == 0 || $this->timestamp == -1)
			{
				$this->timestamp = 0;
				$this->day = 0;
				$this->month = 0;
				$this->year = 0;
				$this->hour = 0;
				$this->minutes = 0;
				$this->seconds = 0;
				$this->string = " N/A ";
				$this->short_string = " N/A ";
			}
		}
		
		function ondrop()
		{
			$this->parse();	
		}
		function onReceive()
		{
			$this->parse();	
		}
	}
