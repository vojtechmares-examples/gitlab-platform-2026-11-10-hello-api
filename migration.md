# Database Migrations

## Diff tools

Schema v1 vs schema v2

```sql
ALTER TABLE ...
```

## SQL migration files (with downtime)

- Entity Framework
- Hybernate
- Flyway

- generuju SQL soubory pro inkrementalni zmeny
- up/down

## SQL migraiton files (without downtime)

- append only

NESMIM:

- prejmovanot sloupec
- prejmenovat tabulku
- zmenit datovy typ sloupce

- nastroj pro migrace hlida, ktere SQL skripty uz byly aplikovane a jiz aplikovane preskoci
- je na vyvojarich, aby si hlidali, kdo ma jakou migraci (ciselny prefix) a pripadne prejmenovali svoje migrace pred mergem do masteru/mainu