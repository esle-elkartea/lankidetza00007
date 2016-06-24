<?php
include ("local.php");
$sf = new scaffold("pedidos_ventanas");
$sf->setCondition("idpedido", $r->pedido_id);
$sf->__query = "SELECT * FROM pedidos_ventanas where idpedido=".$r->pedido_id." ORDER BY id ASC";
header ("content-type:text/xml");
echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
echo "<ventanas>";
foreach ($sf->getList() as $ventana)
{
	$madera = new scaffold("maderas");
	$modelo = new scaffold("modelos");
	$madera->open($ventana["idmadera"]);
	$modelo->open($ventana["idmodelo"]);
	
	echo "
<ventana
id=\"".$ventana["id"]."\" 
unidades=\"".$ventana["unidades"]."\"
madera=\"".$madera->referencia."\"
modelo=\"".$modelo->referencia."\"
hojas=\"".$ventana["hojas"]."\"
ventana_largo=\"".$ventana["ventana_largo"]."\"
ventana_ancho=\"".$ventana["ventana_ancho"]."\"
ventana_grueso=\"".$ventana["ventana_grueso"]."\"
jamba_dm=\"".$ventana["jamba_dm"]."\"
jamba_macizo=\"".$ventana["jamba_macizo"]."\"
jamba_medidas=\"".$ventana["jamba_medidas"]."\"
oscilante=\"".$ventana["oscilante"]."\"
batiente=\"".$ventana["batiente"]."\"
cristal=\"".$ventana["cristal"]."\"
cristal_mate=\"".$ventana["cristal_mate"]."\"
cristal_carglass=\"".$ventana["cristal_carglass"]."\"
cristal_grueso=\"".$ventana["cristal_grueso"]."\"
material_inox=\"".$ventana["material_inox"]."\"
material_laton=\"".$ventana["material_laton"]."\"
/>";	
}
echo "</ventanas>";

