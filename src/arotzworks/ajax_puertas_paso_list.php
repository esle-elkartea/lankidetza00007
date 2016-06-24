<?php
include ("local.php");
$sf = new scaffold("pedidos_paso");
$sf->setCondition("idpedido", $r->pedido_id);
$sf->__query = "SELECT * FROM pedidos_paso where idpedido=".$r->pedido_id." ORDER BY id ASC";
header ("content-type:text/xml");
echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
echo "<puertas_paso>";
foreach ($sf->getList() as $puerta)
{
	$madera = new scaffold("maderas");
	$modelo = new scaffold("modelos");
	$madera->open($puerta["idmadera"]);
	$modelo->open($puerta["idmodelo"]);
	
	echo "
<puerta_paso
id=\"".$puerta["id"]."\" 
unidades=\"".$puerta["unidades"]."\"
madera=\"".$madera->referencia."\"
modelo=\"".$modelo->referencia."\"
puerta_largo=\"".$puerta["puerta_largo"]."\"
puerta_ancho=\"".$puerta["puerta_ancho"]."\"
puerta_grueso=\"".$puerta["puerta_grueso"]."\"
regrueso_dm=\"".$puerta["regrueso_dm"]."\"
regrueso_macizo=\"".$puerta["regrueso_macizo"]."\"
regrueso_medidas=\"".$puerta["regrueso_medidas"]."\"
jamba_dm=\"".$puerta["jamba_dm"]."\"
jamba_macizo=\"".$puerta["jamba_macizo"]."\"
jamba_medidas=\"".$puerta["jamba_medidas"]."\"
mano_izq=\"".$puerta["mano_izq"]."\"
mano_dch=\"".$puerta["mano_dch"]."\"
condena=\"".$puerta["condena"]."\"
material_inox=\"".$puerta["material_inox"]."\"
material_laton=\"".$puerta["material_laton"]."\"
/>";	
}
echo "</puertas_paso>";

