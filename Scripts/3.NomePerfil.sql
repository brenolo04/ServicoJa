-- public."Perfis" definição

-- Drop table

-- DROP TABLE public."Perfis";

CREATE TABLE public."Perfis" (
	"Id" int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	"IdUsuarioIdentity" int8 NOT NULL,
	"Nome" varchar(80) DEFAULT ''::character varying NOT NULL,
	CONSTRAINT "PK_Perfis" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_Perfis_AspNetUsers_IdUsuarioIdentity" FOREIGN KEY ("IdUsuarioIdentity") REFERENCES public."AspNetUsers"("Id")
);
CREATE INDEX "IX_Perfis_IdUsuarioIdentity" ON public."Perfis" USING btree ("IdUsuarioIdentity");