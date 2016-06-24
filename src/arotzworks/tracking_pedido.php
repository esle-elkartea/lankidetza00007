<?php
include ("local.php");
$sf = new scaffold("pedidos");
$sf->setCondition("tracking","AW/".$r->prefix."/".strtoupper($r->suffix));
$list = $sf->getList();
if ($list[0]["id"] == "")
{
	header ("location:tracking.php?error=1&prefix=".$r->prefix."&suffix=".$r->suffix);
	die();
} 

$p = new aworkstrackpage();
$sf->open($list[0]["id"]);
$ew = new estadolistwidget();
$ew->drop($sf);
$d_pedido = new date(); $d_pedido->timestamp = $sf->fecha_pedido ; $d_pedido->parse();
$d_entrega_estimada = new date(); $d_entrega_estimada->timestamp = $sf->fecha_entrega_prevista; $d_entrega_estimada->parse();
$d_entrega = new date(); $d_entrega->timestamp = $sf->fecha_entrega; $d_entrega->parse();

$out .="
<br /><br /><center><div style=\"background:rgb(239,237,222);width:640px;padding:12px;border:outset 2px;\">
<center><br />
<h2>Pedido ".$sf->tracking."</h2>
<table style=\"width:560px;\"><tr><td style=\"width:220px;\">Pedido</td><td> <b>".$sf->tracking;
$out .="</td></tr><tr><td> Cliente </td><td><b>".$sf->cliente;
$out .="</td></tr><tr><td>  Referencia Cliente </td><td> <b>".$sf->referencia;
$out .="</td></tr><tr><td> Estado </td><td>".$ew->draw();
$out .="</td></tr><tr><td> Fecha Pedido</td><td>".$d_pedido->string;
$out .="</td></tr><tr><td> Fecha Entrega Estimada</td><td>".$d_entrega_estimada->string;
$out .="</td></tr><tr><td> Fecha Entrega</td><td>".$d_entrega->string;
$out .="</table>

";
$p->addWidget(html::write($out));
$ev = new eventviewer();
$ev->drop($sf);
$p->addWidget($ev);
$p->addWidget(html::write("<br /><br /><br ></div>"));
$p->draw();
?>