CREATE DATABASE qeamDB;
USE qeamDB;

-- Initialize table content
CREATE TABLE attendees (
	id INT AUTO_INCREMENT,
    uid  VARCHAR(25) NOT NULL,
    fn VARCHAR (24) NOT NULL,
    mi VARCHAR (2),
    ln VARCHAR (16) NOT NULL,
    membership BIT NOT NULL,
    position BIT NOT NULL,
    institution VARCHAR(64),
    pn VARCHAR (10),
    
    amd1 VARCHAR(100),
    lunchd1 VARCHAR(100),
    pmd1 VARCHAR(100),
    checkind1 VARCHAR(100),
    checkoutd1 VARCHAR(100),
    
    amd2 VARCHAR(100),
    lunchd2 VARCHAR(100),
    pmd2 VARCHAR(100),
    checkind2 VARCHAR(100),
    checkoutd2 VARCHAR(100),
    
    amd3 VARCHAR(100),
    lunchd3 VARCHAR(100),
    pmd3 VARCHAR(100),
    checkind3 VARCHAR(100),
    checkoutd3 VARCHAR(100),
    
    PRIMARY KEY (id)
);

-- Update pmd1 columnw with current time.
UPDATE attendees
SET pmd1 = NOW()
WHERE id = 1;

-- Reset the attendances and stuffs for x Day
UPDATE attendees 
SET 
    amd1 = NULL,
    lunchd1 = NULL,
    pmd1 = NULL,
    checkind1 = NULL,
    checkoutd1 = NULL,
    
    amd2 = NULL,
    lunchd2 = NULL,
    pmd2 = NULL,
    checkind2 = NULL,
    checkoutd2 = NULL,
    
    amd3 = NULL,
    lunchd3 = NULL,
    pmd3 = NULL,
    checkind3 = NULL,
    checkoutd3 = NULL;

-- Delete all records in table
DELETE FROM attendees;

-- Delete table content created
DROP TABLE attendees;