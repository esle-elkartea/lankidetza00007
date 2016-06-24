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
    
    $Id: page.class.php,v 1.3 2005/12/27 16:25:15 luis Exp $
*/    
	class page extends oowidget
	{
		var $stylesheet="css/style.css";
		var $title;
		var $meta;
		
		
			function page()
			{
				
				
			}
			
			function addmeta($key,$value)
			{
				$m["key"] = $key;
				$m["value"] = $value;
				$this->meta[] = $m;	
				
			}
		
			function draw()
			{
				debug ($this,"Drawing Page");
				echo "
<html>
	<head>
		<title>".$this->title."</title>
			<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>
			<link rel=\"stylesheet\" href=\"".$this->stylesheet."\" media=\"screen\" />
                        <link rel=\"stylesheet\" href=\"".$this->stylesheet.".print\" media=\"print\" />";
				for ($i=0;$i<count($this->meta);$i++)
					echo "<meta name=\"".$this->meta[$i]["key"]."\" value=\"".$this->meta[$i]["value"]."\" />";
				echo "</head>
	<body>
	<script>
	document.onmousedown = function (event) {
  		var tagName = event.target.tagName;
  		return (tagName == \"A\" || tagName == \"BUTTON\" || tagName == \"INPUT\" || tagName == \"TEXTAREA\" || tagName == \"SELECT\");
	}
	</script>
	";
	
				foreach ($this->__widgetfields as $k)
				{
						$out .= $k->draw()."\n";
						unset($k);
				}
				echo $out;
				echo "
	</body>
</html>";	
			}
		
	}









?>
