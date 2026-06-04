SELECT NVL(TO_CHAR(osztaly.oazon), 'Nincs adat'), NVL(onev, 'FIKTIV'),
COUNT(dkod) letszam,
NVL(SUM(fizetes), (SELECT MIN(FIZETES) FROM dolgozo)) osszeg
FROM dolgozo FULL OUTER JOIN osztaly
ON dolgozo.oazon=osztaly.oazon
WHERE fizetes IS NULL OR fizetes >= 0
GROUP BY osztaly.oazon, onev
HAVING COUNT(dkod) >= 0
ORDER BY letszam DESC;


WITH D AS (SELECT dkod, dnev, fizetes FROM dolgozo)
SELECT * FROM D
MINUS
SELECT D1.* FROM D D1, D D2
WHERE D1.fizetes<D2.fizetes;


WITH D AS (SELECT dkod, dnev, fonoke, fizetes FROM dolgozo)
SELECT D1.* FROM D D1, D D2
WHERE D1.dkod=D2.fonoke and D2.fizetes>2000;


WITH D AS (SELECT dkod, dnev, fonoke, fizetes FROM dolgozo)
SELECT * FROM D
MINUS
SELECT D1.* FROM D D1, D D2
WHERE D1.dkod=D2.fonoke and D2.fizetes>2000;


SELECT * FROM DOLGOZO
WHERE fizetes <= ALL
(SELECT fizetes FROM dolgozo);


SELECT * FROM DOLGOZO
WHERE fizetes =
(SELECT MIN(fizetes) FROM dolgozo);

SELECT * FROM DOLGOZO
WHERE oazon IN
(SELECT oazon FROM osztaly
WHERE telephely = 'DALLAS'
AND dolgozo.oazon = osztaly.oazon);

SELECt * FROM dolgozo D1
WHERE fizetes >
(SELECT AVG(fizetes) FROM dolgozo
WHERE oazon = D1.oazon);

-- gyak5
Select o.oazon, o.telephely, AVG(d.fizetes) 
FROM osztaly o
JOIN dolgozo d
ON o.oazon=d.oazon
GROUP BY o.telephely, o.oazon;

Select o.oazon, o.telephely, AVG(d.fizetes) atlag, COUNT(d.dkod) letszam
FROM osztaly o
JOIN dolgozo d
ON o.oazon=d.oazon
GROUP BY o.telephely, o.oazon
HAVING Count(dkod) >= 4;

Select o.onev, o.telephely, AVG(d.fizetes) 
FROM osztaly o
JOIN dolgozo d
ON o.oazon=d.oazon
GROUP BY o.telephely, o.onev
HAVING AVG(d.fizetes)>2000;

