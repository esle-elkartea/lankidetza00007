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
    
    $Id: currency.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
        class currency extends oowidget
        {
                var $ammount = 0.00;
                var $__currency = "&euro;";
                function currency($ammount="0.00")
                {
                        $this->ammount = $ammount;

                }
                
                function onReceive()
                {
                	$o = $this->popMailBox();
                	$this->ammount = $o->ammount;
                	
                }
                
                function draw()
                {
                        $this->ammount = number_format($this->ammount,2,".",",");
                        if ($this->ammount > 0)
                        return $this->ammount ." ". $this->__currency;
                        return "<font color=\"red\">".$this->ammount." ".$this->__currency."</font>";
                }
        }
