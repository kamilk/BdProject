CREATE SEQUENCE filo.SERIE_ID_SEQ 
    NOCACHE 
    ORDER ;

CREATE OR REPLACE TRIGGER filo.SERIE_ID_TRG 
BEFORE INSERT ON filo.SERIE
FOR EACH ROW 
WHEN (NEW.ID IS NULL) 
BEGIN 
    SELECT filo.SERIE_ID_SEQ.NEXTVAL INTO :NEW.ID FROM DUAL; 
END;
/
