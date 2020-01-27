/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 80019
Source Host           : localhost:3306
Source Database       : mysql

Target Server Type    : MYSQL
Target Server Version : 80019
File Encoding         : 65001

Date: 2020-01-27 19:37:25
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for product
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `categoryId` bigint NOT NULL COMMENT '品类id',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `subtitle` varchar(255) DEFAULT NULL COMMENT '子标题',
  `status` tinyint NOT NULL DEFAULT '0' COMMENT '商品状态（0-正常，1-不可用）',
  `createTime` timestamp NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `updateTime` timestamp NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `index_name` (`name`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
