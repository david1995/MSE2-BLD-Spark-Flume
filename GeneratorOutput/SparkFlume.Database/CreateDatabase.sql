use Products;

CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Products` (
    `Id` int NOT NULL,
    `Minute` datetime NOT NULL,
    `Purchases` bigint NOT NULL,
    `Revenue` decimal(18, 2) NOT NULL,
    `Views` bigint NOT NULL,
    PRIMARY KEY (`Id`, `Minute`)
);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20180620163425_InitialDatabase', '2.0.3-rtm-10026');

