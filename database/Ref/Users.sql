CREATE TABLE "Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "Provider" TEXT NOT NULL,
    "ProviderId" TEXT NOT NULL,
    "Username" TEXT NOT NULL
)