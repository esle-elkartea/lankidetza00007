<?php
include ("local.php");
$sf = new scaffold("pedidos_paso");
$sf->fromObject($r);
$sf->condena = $r->condena == "1" ? 1 : 0;
$sf->insert();
echo $r->condena;
print_arr(errorobject::errorlist());
echo "ok";
?>