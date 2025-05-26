CREATE TABLE "Projects" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "OwnerId" TEXT NOT NULL,
    CONSTRAINT "FK_Projects_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE CASCADE
)