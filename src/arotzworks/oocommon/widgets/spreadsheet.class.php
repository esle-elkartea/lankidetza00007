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
    
    $Id: spreadsheet.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
		class spreadsheet extends oowidget {
			
			var $cells;
			var $width="100%";
			
			function spreadsheet($cols=8,$rows=8)
			{
					$letterarr = array ('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y'.'Z');
					$this->cols = $cols;
					$this->rows = $rows;
					for ($x =0;$x<$cols;$x++)
					{
						for ($y = 0;$y<$rows;$y++)
						{
							$cellname = $letterarr[$x].$y;
							$this->cells[$cellname]["widget"] = " ";
							$this->cells[$cellname]["style"] = " ";
							$this->cells[$cellname]["value"] = " ";
							$this->cells[$cellname]["__params"]["col"] = $letterarr[$x];
							$this->cells[$cellname]["__params"]["ncol"] = $x;
							$this->cells[$cellname]["__params"]["row"] = $y;
							$this->cells[$cellname]["colspan"] = 1;
						}	
					}
			}			
			
			function sum($source,$dest)
			{
				if ($source["__params"]["col"] == $dest["__params"]["col"] )
				{
					$itemsrc = $source["__params"]["row"];
					$itemdst = $dest["__params"]["row"];
					$itemcol = $source["__params"]["col"];
					
					if ($itemsrc > $itemdst)
					{ //Interchange
						$tmp = $itemsrc;
						$itemsrc = $itemdst;
						$itemdst = $itemsrc;	
					}
					
					// Vertical Sum
					// Get column and iterate
					for ($i = $itemsrc ; $i<$itemdst;$i++)
					{
						$cellname = $itemcol.$i;
						$sum = $this->cells[$cellname]["value"] + $sum;
											
					}
					return $sum;
				}
					
				if ($source["__params"]["row"] == $dest["__params"]["row"] )
				{
					$letterarr = array ('A','B','C','D'.'E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y'.'Z');
									// Horizontal Sum
					$itemsrc = $source["__params"]["ncol"];
					$itemdst = $dest["__params"]["ncol"];
					$itemrow = $source["__params"]["row"];
					
					if ($itemsrc > $itemdst)
					{ //Interchange
						$tmp = $itemsrc;
						$itemsrc = $itemdst;
						$itemdst = $itemsrc;	
					}
					
					// Vertical Sum
					// Get column and iterate
					for ($i = $itemsrc ; $i<$itemdst;$i++)
					{
						$l = $letterarr[$i] ;
						$cellname = $l.$itemrow;
						$sum = $this->cells[$cellname]["value"] + $sum;
					}
					return $sum;	
				}
			}
		
			function draw()
			{
					$letterarr = array ('A','B','C','D'.'E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y'.'Z');
					$out ="<table border=\"1\" style=\"width:".$this->width."\">";
					for ($y =0;$y<$this->cols;$y++)
					{
						$out .= "<tr>";
						for ($x = 1;$x<$this->rows;$x++)
						{
							$cellname = $letterarr[$y].$x;
							$out .="<td colspan=\"".$this->cells[$cellname]["colspan"]."\">";
							$out .=(is_object($this->cells[$cellname]["widget"]) ? $this->cells[$cellname]["widget"]->draw() : $this->cells[$cellname]["value"]);
							$out .="&nbsp;</td>";
							if ($this->cells[$cellname]["colspan"] > 1)
							{	
								$span = $this->cells[$cellname]["colspan"] - 1;
								$x +=$span;
								continue;
							}
						}	
						$out .= "</tr>";
					}	
					$out .="</table>";
			return $out;
			}
						
		}
		
		?>