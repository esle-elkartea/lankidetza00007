<?php
	class infobox extends oowidget
	{

		function infobox($title,$text,$info=false)
		{
			$this->title = $title;
			$this->text = $text;
			$this->info = $info;
		}	

		function draw()
		{
			$out ="<div id=\"infobox\" style=\"background:rgb(255,255,255);left:0%;top:0%;width:100%;border:solid 1px;margin:2px;overflow:hidden\">
				<div style=\"width:100%;height:18px;font-size:14px;padding:4px;font-weight:bold;border:outset 1px;color:rgb(255,255,255);background:".($this->info ? "rgb(0,210,0);" : "rgb(255,0,0);")."\">
				".$this->title."

				
				</div>
				<div style=\"padding:12px;font-size:11px;font-weight:bold;\">
				".$this->text."<br />

				";
			global $__gl_errors;
			for ($i=0;$i<count($__gl_errors);$i++)
			{
				$out .="".$__gl_errors[$i]->string.($__gl_errors[$i]->param ? " ".$__gl_errors[$i]->param : "" )."<br />";				
			}	
				global $verb;
				$out .="
				<br />
				";
					$out .="<button id=\"infobutton\" accesskey=\"P\" onclick=\"javascript:infobox_diffuse()\">Ace<u>p</u>tar</button>";
				$out .="
				</div></div>
				<script>
				var h=100;		
				document.getElementById('infobutton').focus();
				function diffuseinfobox()
				{
					h-=5;
					document.getElementById('infobox').style.MozOpacity=(h/150);
					document.getElementById('infobox').style.height = h;
					if (h > 0) setTimeout('diffuseinfobox()',10);
					if (h ==0) document.getElementById('infobox').style.display='none'; 
					
				}
				function infobox_diffuse()
				{
				setTimeout('diffuseinfobox()',400);
				}
			</script>
			";
			return $out;
		}
	}
?>