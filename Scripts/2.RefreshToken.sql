CREATE TABLE public."RefreshToken" (
	"Id" uuid NOT NULL,
	"Token" varchar(200) NOT NULL,
	"IdUsuarioIdentity" int8 NOT NULL,
	"ExpiresOnUtc" timestamp with time zone NOT NULL,
	CONSTRAINT "PK_RefreshToken" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_RefreshToken_AspNetUsers_IdUsuarioIdentity" FOREIGN KEY ("IdUsuarioIdentity") REFERENCES public."AspNetUsers"("Id")
);
CREATE UNIQUE INDEX "IX_RefreshToken_Token" ON public."RefreshToken" USING btree ("Token");