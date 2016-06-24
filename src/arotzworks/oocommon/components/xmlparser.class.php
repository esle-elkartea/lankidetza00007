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
    
    $Id: xmlparser.class.php,v 1.3 2005/12/27 16:25:15 luis Exp $
*/    
	
	class xpath
	{
		var $xml;
		function xpath($xml)
		{
			$this->xml = $xml;
		}

		function GetElementByName ($start, $end) {
			$startpos = strpos($this->xml, $start);
			if ($startpos === false) {
			return false;
			}
			$endpos = strpos($this->xml, $end);
			$endpos = $endpos+strlen($end);
			$endpos = $endpos-$startpos;
			$endpos = $endpos - strlen($end);
			$tag = substr ($this->xml, $startpos, $endpos);
			$tag = substr ($tag, strlen($start));
			return $tag;
		}

		function get($XPath) {
		$XPathArray = explode("/",$XPath);
		$node = $this->xml;
		while (list($key,$value) = each($XPathArray)) {
			$node = $this->GetElementByName("<".$value.">", "</".$value.">");
		}
		return $node;
		}
	}

	function startElement($parser, $name, $attrs)
	{
		global $obj;
   		eval('$test=isset('.$obj->tree.'->'.$name.');');
   		if ($test)
		{
     			eval('$tmp='.$obj->tree.'->'.$name.';');
     			eval('$arr=is_array('.$obj->tree.'->'.$name.');');
     			if (!$arr)
			{
       				eval('unset('.$obj->tree.'->'.$name.');');
       				eval($obj->tree.'->'.$name.'[0]=$tmp;');
       				$cnt = 1;
     			} else {
       				eval('$cnt=count('.$obj->tree.'->'.$name.');');
     			}
     			$obj->tree .= '->'.$name."[$cnt]";
   		} else {
     			$obj->tree .= '->'.$name;
   		}

		if (count($attrs))
		{
       			eval($obj->tree.'->attr=$attrs;');
   		}
	}

	function endElement($parser, $name)
	{
		global $obj;
   		// Strip off last ->
   		for($a=strlen($obj->tree);$a>0;$a--)
		{
       			if (substr($obj->tree, $a, 2) == '->')
			{
	           		$obj->tree = substr($obj->tree, 0, $a);
           			break;
       			}
   		}
	}

	function characterData($parser, $data)
	{
		global $obj;eval($obj->tree.'->data=\''.$data.'\';');
		//$obj->tree->$data = $data;
	}


class xmlparser
{
	var $xml;
	var $obj;
	var $tree;
	function xmlparser($xml)
	{
		if (file_exists($xml))
		{
			$xml = implode("\n",file($xml));
		}   

		global $obj;
		$obj->tree = '$obj->xml';
		$obj->xml = '';
		$xml_parser = xml_parser_create();
		xml_set_element_handler($xml_parser, "startElement", "endElement");
		xml_parser_set_option($xml_parser,XML_OPTION_CASE_FOLDING,0);
		xml_set_character_data_handler($xml_parser, "characterData");
			if (!xml_parse($xml_parser, $xml,1))
			{
       				ooerror("XML","<b>XML error:".xml_error_string(xml_get_error_code($xml_parser))." at line ".xml_get_current_line_number($xml_parser));
                   	}
		xml_parser_free($xml_parser);
		$this->tree = $obj->xml;
		$obj ="";
		
	}
}