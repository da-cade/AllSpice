CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS recipes(
  id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  picture varchar(255),
  title varchar(255),
  subtitle varchar(255),
  category varchar(255),
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId)
    REFERENCES accounts(id)
    ON DELETE CASCADE
)default charset utf8;

CREATE TABLE IF NOT EXISTS ingredients(
  id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
  name varchar(255),
  quantity varchar(255),
  recipeId INT NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId)
    REFERENCES accounts(id)
    ON DELETE CASCADE,
  FOREIGN KEY (recipeId)
    REFERENCES recipes(id)
    ON DELETE CASCADE
)default charset utf8;

CREATE TABLE IF NOT EXISTS steps(
  id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
  position INT,
  body varchar(255),
  recipeId INT NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId)
    REFERENCES accounts(id)
    ON DELETE CASCADE,
  FOREIGN KEY (recipeId)
    REFERENCES recipes(id)
    ON DELETE CASCADE
)default charset utf8;

CREATE TABLE IF NOT EXISTS favorites(
  id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  recipeId INT NOT NULL,
  accountId VARCHAR(255) NOT NULL,
  FOREIGN KEY (accountId)
    REFERENCES accounts(id)
    ON DELETE CASCADE,
  FOREIGN KEY (recipeId)
    REFERENCES recipes(id)
    ON DELETE CASCADE
)default charset utf8;
