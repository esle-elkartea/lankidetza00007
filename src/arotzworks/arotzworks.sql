CREATE TABLE pedidos (
id int unsigned not null auto_increment,
cliente text,
tracking varchar(255),
referencia text,
tipo_pedido int,
fecha_pedido text,
fecha_entrega_prevista int,
fecha_entrega text,
observaciones text,
otro_material text,
estado int,
primary key(id)
);

CREATE TABLE pedidos_eventos(
id int unsigned not null auto_increment,
idpedido int,
fecha int,
descripcion text,
fecha_entrega_prevista int,
estado_viejo int,
estado_nuevo int,
primary key(id));

CREATE TABLE pedidos_paso(
id int unsigned not null auto_increment,
idpedido int,
unidades int,
idmadera int,
idmodelo int,
puerta_largo float,
puerta_ancho float,
puerta_grueso float,
regrueso_dm bool,
regrueso_macizo bool,
regrueso_medidas text,
jamba_dm bool,
jamba_macizo bool,
jamba_medidas text,
mano_izq bool,
mano_dch bool,
condena bool,
material_inox bool,
material_laton bool,
primary key(id));

CREATE TABLE pedidos_entrada(
id int unsigned not null auto_increment,
idpedido int,
unidades int,
idmadera int,
idmodelo int,
puerta_largo float,
puerta_ancho float,
puerta_grueso float,
regrueso_dm bool,
regrueso_macizo bool,
regrueso_medidas text,
jamba_dm bool,
jamba_macizo bool,
jamba_medidas text,
mano_izq bool,
mano_dch bool,
cerradura_sol bool,
cerradura_nor bool,
bisagra_larga bool,
bisagra_3cortas bool,
material_inox bool,
material_laton bool,
primary key(id));

CREATE TABLE pedidos_ventanas (
id int unsigned not null auto_increment,
idmodelo int,
idmadera int,
unidades int,
ventana_largo float,
ventana_ancho float,
ventana_grueso float,
jamba_dm bool,
jamba_macizo bool,
jamba_medidas text,
oscilante bool,
batiente bool,
material_inox bool,
material_laton bool,
cristal bool,
cristal_mate bool,
cristal_carglass bool,
cristal_grueso float,
primary key (id));


CREATE TABLE modelos (
id int unsigned not null auto_increment,
referencia varchar(255),
stock int,
primary key(id)
);

CREATE TABLE maderas (
id int unsigned not null auto_increment,
referencia varchar(255),
stock int,
primary key(id));

CREATE TABLE materiales(
id int unsigned not null auto_increment,
referencia text,
stock int,
primary key(id));

