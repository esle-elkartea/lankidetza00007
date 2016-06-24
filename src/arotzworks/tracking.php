<?php
include ("local.php");
$p = new aworkstrackpage();
if ($r->error)
{
	$out .="<center><h2 style=\"color:red;position:absolute;left:25%;width:50%;\">Numero de pedido incorrecto<br /><span style=\"font-size:14px\">Revise su n&uacute;mero de pedido y vuelva a intentarlo</span></h2>";
}
$out .="
<form method=\"post\" action=\"tracking_pedido.php\">
<table style=\"width:100%;height:80%;\">
<tr style=\"height:80%\">
<td style=\"width:25%\"></td><td style=\"width:50%;\"><center>
<div style=\"font-size:32px;background:rgb(230,230,230);height:300px;border:outset 2px;\">
<br />
<b>Seguimiento de Pedido</b><br />
<span style=\"font-size:14px;\">
Introduzca n&uacute;mero de seguimiento de pedido proporcionado.<br />
</span><br />
<div style=\"border:inset 1px;width:440px;padding:12px;background:white;\">
AW/<input name=\"prefix\" value=\"".$r->prefix."\" type=\"text\" size=\"3\" style=\"font-size:32px;border:dotted 2px;text-align:center;\">/<input  value=\"".$r->suffix."\" name=\"suffix\" type=\"text\" size=\"6\"  style=\"text-align:center;font-size:32px;border:dotted 2px;\"><button style=\"font-weight:bold;background:#55ff55;width:56px;height:52px;vertical-align:middle;margin-top:-10px;padding-top:6px;\" onclick=\"document.forms[0].submit()\"><img src=\"icons/16x16/arrow_switch.png\"></button>
</div>
<span style=\"font-size:9px;\">
<br />
<div style=\"padding:18px;text-align:justify;\">
Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Ut tincidunt vehicula nibh. Donec massa. Sed in mauris in arcu accumsan gravida. Vestibulum pretium felis eget nibh congue venenatis. Aenean aliquam. Sed placerat sapien sit amet sapien. Cras nisi diam, lacinia sit amet, rutrum ornare, ultricies ut, justo. Mauris sit amet ligula. Mauris sit amet mauris et dolor ultricies dapibus. Curabitur in massa vulputate felis sollicitudin lacinia.
</span>
</div>
</div>
</td>
<td style=\"width:25%\">
</td>
</tr>
</table>
</form>
";
$p->addWidget(html::write($out));
$p->draw();