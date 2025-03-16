CREATE TABLE IF NOT EXISTS ClusterMembership (
    SiloAddress text NOT NULL PRIMARY KEY,
    HostName text NOT NULL,
    SiloName text NOT NULL,
    Status text NOT NULL,
    StartTime timestamp without time zone NOT NULL,
    EndTime timestamp without time zone,
    ETag text NOT NULL
);

CREATE TABLE IF NOT EXISTS SiloStatus (
    SiloAddress text NOT NULL PRIMARY KEY,
    HostName text NOT NULL,
    SiloName text NOT NULL,
    Status text NOT NULL,
    StartTime timestamp without time zone NOT NULL,
    EndTime timestamp without time zone,
    ETag text NOT NULL
);
