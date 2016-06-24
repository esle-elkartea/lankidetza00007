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
    
    $Id: tree.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    

class tree extends oowidget {
	var $foldertable = "folders";
	var $parentfield = "idparent";
	var $titlefield = "name";
	var $height="620px";
	var $target;

	function tree($target) {

	}

	function rfoldertree($idparent) {
		$db = new database();
		$title = $this->titlefield;
		$db->query("SELECT * FROM ".$this->foldertable."  where ".$this->parentfield."=".$idparent);
		if ($db->rows[0]["id"])
			for ($i = 0; $i < count($db->rows); $i ++) {
				$out .= "<div><img src=\"icons/16/folder.png\" onclick=\"javascript:collapse('folder_".$db->rows[$i]["id"]."')\" id=\"ifolder_".$db->rows[$i]["id"]."\"><span onclick=\"javascript:fcollapse('folder_".$db->rows[$i]["id"]."',".$db->rows[$i]["id"].")\")\">".$db->rows[$i][$title]." </span>
									<div style=\"display:none;padding-left:15px;\" id=\"folder_".$db->rows[$i]["id"]."\">";
				$out .= $this->rfoldertree($db->rows[$i]["id"]);
				$out .= "</div></div>";
			}
		return $out;
	}

	function foldertree() {
		$db = new database();
		$title = $this->titlefield;
		$db->query("SELECT * FROM ".$this->foldertable."  where ".$this->parentfield."=0");
		$out .= "<div class=\"foldertree\" style=\"padding:0px;margin:0px;\">
				
					";
		
		if ($db->rows[0]["id"])
			for ($i = 0; $i < count($db->rows); $i ++) {
				$out .= "<div><img src=\"icons/16/folder.png\" onclick=\"javascript:collapse('folder_".$db->rows[$i]["id"]."')\" id=\"ifolder_".$db->rows[$i]["id"]."\"><span onclick=\"javascript:fcollapse('folder_".$db->rows[$i]["id"]."',".$db->rows[$i]["id"].")\">".$db->rows[$i][$title]."</span>
										<div style=\"display:none;padding-left:15px;\" id=\"folder_".$db->rows[$i]["id"]."\">";
				$out .= $this->rfoldertree($db->rows[$i]["id"]);
				$out .= "	</div>
									</div>";
			}
		$out .= "</div>";
		return $out;
	}

	function drawWidget() {
		return $this->foldertree();
	}

}
