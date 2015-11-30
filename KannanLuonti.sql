-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema G7934
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `G7934` ;

-- -----------------------------------------------------
-- Schema G7934
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `G7934` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `G7934` ;

-- -----------------------------------------------------
-- Table `G7934`.`Person`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `G7934`.`Person` (
  `idPerson` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idPerson`),
  UNIQUE INDEX `idHenkilot_UNIQUE` (`idPerson` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `G7934`.`Sport`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `G7934`.`Sport` (
  `idSport` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idSport`),
  UNIQUE INDEX `idLajit_UNIQUE` (`idSport` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `G7934`.`Accoplishment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `G7934`.`Accoplishment` (
  `idAccoplishmnet` INT NOT NULL AUTO_INCREMENT,
  `Duration` INT NOT NULL,
  `Date` DATETIME NOT NULL,
  `Person` INT NOT NULL,
  `Sport` INT NOT NULL,
  PRIMARY KEY (`idAccoplishmnet`),
  UNIQUE INDEX `idSuoritukset_UNIQUE` (`idAccoplishmnet` ASC),
  INDEX `fk_Accoplishment_Person1_idx` (`Person` ASC),
  INDEX `fk_Accoplishment_Sport1_idx` (`Sport` ASC),
  CONSTRAINT `fk_Accoplishment_Person1`
    FOREIGN KEY (`Person`)
    REFERENCES `G7934`.`Person` (`idPerson`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Accoplishment_Sport1`
    FOREIGN KEY (`Sport`)
    REFERENCES `G7934`.`Sport` (`idSport`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
