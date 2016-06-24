<?php
include ("local.php");
$sf = new scaffold("pedidos_entrada");
$sf->fromObject($r);
$sf->insert();
echo $r->condena;
print_arr(errorobject::errorlist());
echo "ok";
?>