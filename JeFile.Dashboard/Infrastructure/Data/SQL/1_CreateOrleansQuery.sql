DO $$ 
BEGIN
    -- Проверяем, существует ли таблица
    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'orleansquery') THEN
        CREATE TABLE OrleansQuery (
            QueryKey VARCHAR(64) NOT NULL PRIMARY KEY,
            QueryText TEXT NOT NULL
        );
    END IF;
END $$;
