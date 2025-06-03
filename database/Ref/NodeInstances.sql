CREATE TABLE "NodeInstances" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_NodeInstances" PRIMARY KEY,
    "ProjectId" TEXT NOT NULL,
    "NodeTypeId" TEXT NOT NULL,
    "ConfigurationJson" TEXT NOT NULL,
    "PositionX" REAL NOT NULL,
    "PositionY" REAL NOT NULL,
    CONSTRAINT "FK_NodeInstances_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
)