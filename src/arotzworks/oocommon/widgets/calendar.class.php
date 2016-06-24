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
    
    $Id: calendar.class.php,v 1.3 2006/01/03 09:00:48 luis Exp $
*/    
class calendar extends oowidget {
	var $days;
	var $month;
	var $year;
	var $href;
	var $css_body = "calendar_body";
	var $css_header = "calendar_header";
	var $css_handles = "calendar_handles";
	var $css_weekdays = "calendar_weekdays";
	var $css_day = "calendar_day";
	var $css_dayselected = "calendar_dayselected";
	var $css_daydisabled = "calendar_daydisabled";
	var $css_dayhighlighted = "calendar_dayhighlighted";
	function calendar($href="") {
		$this->href = $href;
	}

	function toggleDay($day) {
		$this->days[$day] = "DISABLED";
	}

	function drawWidget() {

		$object = $this->popMailBox();
		$this->month = $object->month;
		$this->year = $object->year;

		$month = $this->month;
		$year = $this->year;
		
		if (strpos($this->href,"?") === false)
		{
			$basehref = $this->href."?calendar=1";		
		} else {
			$basehref = $this->href;
		}
		 
		// Por si alguien lo quiere reusar ... aqui lo tiene, el widget del mes.
		// Ha costado lo suyo :D
		$locale = $object->locale;
		;
		setlocale(LC_TIME, $locale);
		// Jan 1 1970 era jueves, eso lo sabemos de por si,
		//  por lo cual podemos calcular que el Jan 2 1970 era viernes, el Jan 3 1970 sabado ...
		// siendo el segundo 0 de la epoca Unix el Jan 1 1970, sabemos que ...
		$l_j = ucfirst(strftime("%a", 0));
		// 24*60*60 segundos = 1 dia, luego esos segundos marcan el Viernes 2 de Enero de 1970
		$l_v = ucfirst(strftime("%a", 24 * 60 * 60));
		// 2*24*60*60 = 2 dias, es decir Sabado 3 de Enero de 1970 , etc etc etc
		$l_s = ucfirst(strftime("%a", 2 * 24 * 60 * 60));
		$l_d = ucfirst(strftime("%a", 3 * 24 * 60 * 60));
		$l_l = ucfirst(strftime("%a", 4 * 24 * 60 * 60));
		$l_m = ucfirst(strftime("%a", 5 * 24 * 60 * 60));
		$l_x = ucfirst(strftime("%a", 6 * 24 * 60 * 60));

		$month_locale = strftime("%B", mktime(0, 0, 0, $month +1, 0, $year));
		$month_locale = ucfirst($month_locale);
		$date = getdate(mktime(0, 0, 0, $month +1, 0, $year));
		$wg_calenda = "
						<table class=\"".$this->css_body."\">
							<tr>
								<td colspan=\"7\" class=\"".$this->css_header."\">
								<center><a class=\"".$this->css_handles."\" href=\"".$basehref."&year=".$year."&month=". ($month -1)."&day=1\">&lt;</a>
								&nbsp;&nbsp;&nbsp;&nbsp;".$month_locale."-".$date["year"]."
								&nbsp;&nbsp;&nbsp;&nbsp;
								<a class=\"".$this->css_handles."\" href=\"".$basehref."&year=".$year."&month=". ($month +1)."&day=1\">&gt;</a></center>
								</td>
							</tr>
							<tr>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_l."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_m."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_x."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_j."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_v."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_s."
								</td>
								<td class=\"".$this->css_weekdays."\">
								<center>".$l_d."
								</td>
							</tr>
						";
		$md = $date["mday"];
		$r = new request(); // Necesita que haya oocommon !!
		$date2 = getdate(mktime(0, 0, 1, $month, 1, $year));
		$date3 = getdate(time());
		if ($r->day == "") {
			$r->day = $date3["mday"];
		}
		$spoint = $date2["wday"] - 1;
		if ($spoint == -1) {
			$spoint = 6;
		}
		// A partir de aqui, todo es confuso :D
		
		for ($d = 0; $d < $md; $d += 7) {
			$wg_calenda .= "<tr>";
			for ($i = 0; $i < 7; $i ++) {
				$count = 0;
				if ($i < $spoint && $started == 0) {
					$d --;
				} else
					if ($i == $spoint && $d < 1) {
						$started = 1;
						$wg_calenda .= ($spoint != 0 ? "<td colspan=\"". ($spoint)."\"></td>" : "");
					}
				if ($started == 1 || $spoint < 0) {

					$wg_calenda .= "\n\t<td class=\"";
					if ($r->day == $d + $i +1) {
						$class = $this->css_dayselected;
					} else {
						$class = $this->css_day;
					}
					
					if ($i > 4)
					{
						$class = $this->css_daydisabled;
					}
					
					for ($k=0;$k<count($this->highlighted);$k++)
					{
						if ($this->highlighted[$k] == $d + $i + 1)
						{
							$class = $this->css_dayhighlighted;
						}
					}

					$wg_calenda .= $class."\">";
					if ($d + $i < $md) {
						$wg_calenda .= "<a class=\"". ($this->days[$d + $i +1] == "DISABLED" || $i > 4 ? $this->css_daylink_weekend : $this->css_daylink)."\" href=\"".$basehref."&year=".$year."&month=".$month."&day=". ($d + $i +1)."\"><center><font size=\"-4\">". ($d + $i +1)."</font></center></a></td>";
					}
				}
			}
			$wg_calenda .= "</tr>";
		}
		$wg_calenda .= "</table>";
		setlocale(LC_ALL, "C");
		return $wg_calenda;

	}

}
