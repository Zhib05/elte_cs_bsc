Select haz_nev FROM got_karakterek
where vagyon >= 15 * sereg
GROUP BY haz_nev
HAVING COUNT(haz_nev)>= 2;

Select haz_nev, sum(vagyon), sum(NVL(sereg, 1000)) serege from got_karakterek
group by haz_nev
having sum(vagyon) > 20000;


Select nev
from got_karakterek g
join (select haz_nev, max(sereg) sereg from got_karakterek
group by haz_nev) a
on g.sereg = a.sereg and g.haz_nev = a.haz_nev;


SELECT h.motto
FROM got_hazak h
JOIN got_csatak cs ON h.haz_nev = cs.haz_nev
WHERE cs.gyozott = 'nem'
  AND cs.csata_nev IN (
    -- Megkeressük azokat a csatákat, amiknek a résztvevő száma eléri a maximumot
    SELECT csata_nev
    FROM got_csatak
    GROUP BY csata_nev
    HAVING COUNT(haz_nev) = (
        -- Ez az al-lekérdezés adja meg a legmagasabb résztvevői számot
        SELECT MAX(COUNT(haz_nev))
        FROM got_csatak
        GROUP BY csata_nev
    )
  );