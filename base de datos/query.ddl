CREATE DATABASE Sistemas
go

USE Sistemas
go

CREATE TABLE AdjuntosDocumentacion
( 
	idAdjuntosDocumentacion int IDENTITY ( 1,1 ) ,
	archivo               varchar(50)  NULL ,
	idDocumentacion      int  NULL 
)
go



ALTER TABLE AdjuntosDocumentacion
	ADD CONSTRAINT XPKAdjuntosDocumentacion PRIMARY KEY  CLUSTERED (idAdjuntosDocumentacion ASC)
go



CREATE TABLE documentacion
( 
	idDocumentacion      int IDENTITY ( 1,1 ) ,
	nombre_documento     varchar(50)  NULL ,
	descripcion          varchar(50)  NULL ,
	fecha_creacion       datetime  NULL 
)
go



ALTER TABLE documentacion
	ADD CONSTRAINT XPKdocumentacion PRIMARY KEY  CLUSTERED (idDocumentacion ASC)
go



CREATE TABLE dptos
( 
	idDpto               int IDENTITY ( 1,1 ) ,
	nombre_dpto          varchar(50)  NULL 
)
go



ALTER TABLE dptos
	ADD CONSTRAINT XPKdptos PRIMARY KEY  CLUSTERED (idDpto ASC)
go



CREATE TABLE inventarios
( 
	idInventario         int IDENTITY ( 1,1 ) ,
	nombre_inventario    varchar(50)  NULL ,
	cantidad             varchar(50)  NULL ,
	serial               varchar(50)  NULL ,
	ubicacion            varchar(50)  NULL ,
	fecha_creacion       datetime  NULL ,
	idUsuario           int  NULL ,
	idTipoInventario     int  NULL 
)
go



ALTER TABLE inventarios
	ADD CONSTRAINT XPKinventarios PRIMARY KEY  CLUSTERED (idInventario ASC)
go



CREATE TABLE reportes
( 
	idReportes           int IDENTITY ( 1,1 ) ,
	reportado_por        varchar(50)  NULL ,
	fecha_inicio         datetime  NULL ,
	fecha_cierre         datetime  NULL ,
	problema_presentado  varchar(50)  NULL ,
	falla                varchar(100)  NULL ,
	solucion             varchar(100)  NULL ,
	estatus              varchar(10)  NULL ,
	idUsuario            int  NULL ,
	idTipoFalla          int  NULL ,
	idDpto               int  NULL 
)
go



ALTER TABLE reportes
	ADD CONSTRAINT XPKreportes PRIMARY KEY  CLUSTERED (idReportes ASC)
go


CREATE TABLE prestamos
( 
	idPrestamos           int IDENTITY ( 1,1 ) ,
	Aquien_presta         varchar(50)  NULL ,
	fecha_entrega         datetime  NULL ,
	fecha_devolucion      datetime  NULL ,
	nota                  varchar(50)  NULL ,
	nota_devolucion       varchar(50)  NULL ,
	estatus               varchar(10)  NULL ,
	idUsuario             int  NULL 
)
go



ALTER TABLE prestamos
	ADD CONSTRAINT XPKprestamos PRIMARY KEY  CLUSTERED (idPrestamos ASC)
go


CREATE TABLE tipo_falla
( 
	idTipoFalla          int IDENTITY ( 1,1 ) ,
	nombre_falla         varchar(50)  NULL 
)
go



ALTER TABLE tipo_falla
	ADD CONSTRAINT XPKtipo_falla PRIMARY KEY  CLUSTERED (idTipoFalla ASC)
go



CREATE TABLE tipoInventario
( 
	idTipoInventario     int IDENTITY ( 1,1 ) ,
	nombre_tipo          varchar(50)  NULL ,
	descripcion          varchar(50)  NULL 
)
go



ALTER TABLE tipoInventario
	ADD CONSTRAINT XPKtipoInventario PRIMARY KEY  CLUSTERED (idTipoInventario ASC)
go



CREATE TABLE usuarios
( 
	idUsuario           int IDENTITY ( 1,1 ) ,
	nombre               varchar(50)  NULL ,
	apellido             varchar(50)  NULL ,
	correo               varchar(50)  NULL ,
	clave                varchar(50)  NULL ,
	fecha_n              datetime  NULL 
)
go



ALTER TABLE usuarios
	ADD CONSTRAINT XPKusuarios PRIMARY KEY  CLUSTERED (idusuario ASC)
go


CREATE TABLE vitacora
( 
	idVitacora           int IDENTITY ( 1,1 ) ,
	descripcion          varchar(50)  NULL ,
	fecha                datetime  NULL ,
	idUsuario            int  NULL 
)
go



ALTER TABLE vitacora
	ADD CONSTRAINT XPKvitacora PRIMARY KEY  CLUSTERED (idVitacora ASC)
go




ALTER TABLE AdjuntosDocumentacion
	ADD CONSTRAINT R_10 FOREIGN KEY (idDocumentacion) REFERENCES documentacion(idDocumentacion)
		ON DELETE CASCADE 
		ON UPDATE NO ACTION
go




ALTER TABLE inventarios
	ADD CONSTRAINT R_11 FOREIGN KEY (idusuario) REFERENCES usuarios(idusuario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE inventarios
	ADD CONSTRAINT R_12 FOREIGN KEY (idTipoInventario) REFERENCES tipoInventario(idTipoInventario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE reportes
	ADD CONSTRAINT R_7 FOREIGN KEY (idusuario) REFERENCES usuarios(idusuario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE prestamos
	ADD CONSTRAINT R_45 FOREIGN KEY (idusuario) REFERENCES usuarios(idusuario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE reportes
	ADD CONSTRAINT R_8 FOREIGN KEY (idTipoFalla) REFERENCES tipo_falla(idTipoFalla)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE reportes
	ADD CONSTRAINT R_9 FOREIGN KEY (idDpto) REFERENCES dptos(idDpto)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE vitacora
	ADD CONSTRAINT R_56 FOREIGN KEY (idUsuario) REFERENCES usuarios(idUsuario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




