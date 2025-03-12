CREATE OR REPLACE FUNCTION cleanup_defunct_silo_entries() RETURNS void AS $$
BEGIN
    DELETE FROM ClusterMembership WHERE EndTime < now() - INTERVAL '24 hours';
END;
$$ LANGUAGE plpgsql;
