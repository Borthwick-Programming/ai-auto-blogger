CREATE TABLE "NodeConnections" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_NodeConnections" PRIMARY KEY,
    "FromNodeInstanceId" TEXT NOT NULL,
    "FromPortName" TEXT NOT NULL,
    "ToNodeInstanceId" TEXT NOT NULL,
    "ToPortName" TEXT NOT NULL,
    CONSTRAINT "FK_NodeConnections_NodeInstances_FromNodeInstanceId" FOREIGN KEY ("FromNodeInstanceId") REFERENCES "NodeInstances" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_NodeConnections_NodeInstances_ToNodeInstanceId" FOREIGN KEY ("ToNodeInstanceId") REFERENCES "NodeInstances" ("Id") ON DELETE CASCADE
)