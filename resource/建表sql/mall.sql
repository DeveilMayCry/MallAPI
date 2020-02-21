/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 80011
Source Host           : localhost:3306
Source Database       : mall

Target Server Type    : MYSQL
Target Server Version : 80011
File Encoding         : 65001

Date: 2020-02-21 20:40:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for category
-- ----------------------------
DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `parentId` bigint(20) DEFAULT NULL COMMENT '父级id',
  `name` varchar(255) NOT NULL COMMENT '名称',
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态（0-正常，1-不可用）',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '创建时间',
  `updateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `index_name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of category
-- ----------------------------
INSERT INTO `category` VALUES ('1', null, '手机', '0', '2020-02-11 15:28:30', '2020-02-11 15:28:30', null);
INSERT INTO `category` VALUES ('2', null, '移动座机', '0', '2020-02-11 15:28:38', '2020-02-11 15:28:38', null);

-- ----------------------------
-- Table structure for permission
-- ----------------------------
DROP TABLE IF EXISTS `permission`;
CREATE TABLE `permission` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL COMMENT '权限名称',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of permission
-- ----------------------------
INSERT INTO `permission` VALUES ('1', '查询-产品list', '0');

-- ----------------------------
-- Table structure for product
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `categoryId` bigint(20) NOT NULL COMMENT '品类id',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `subtitle` varchar(255) DEFAULT NULL COMMENT '子标题',
  `price` decimal(10,2) NOT NULL COMMENT '商品价格',
  `mainImage` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '商品图片',
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '商品状态（0-正常，1-不可用）',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `updateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `index_name` (`name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of product
-- ----------------------------
INSERT INTO `product` VALUES ('1', '1', 'iphone7', '双十一促销', '7199.22', 'mainimage.jpg', '0', '2020-02-11 15:32:42', '2020-02-11 15:32:42', null);
INSERT INTO `product` VALUES ('2', '1', 'oppo R8', 'oppo促销进行中', '2999.00', 'mainimage2.jpg', '0', '2020-02-11 15:33:18', '2020-02-11 15:33:18', null);

-- ----------------------------
-- Table structure for roleandpermission
-- ----------------------------
DROP TABLE IF EXISTS `roleandpermission`;
CREATE TABLE `roleandpermission` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `roleId` bigint(255) NOT NULL COMMENT '角色id',
  `permissionId` bigint(20) NOT NULL COMMENT '权限id',
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '正常-0 不可用-1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of roleandpermission
-- ----------------------------
INSERT INTO `roleandpermission` VALUES ('1', '4', '1', '0');
INSERT INTO `roleandpermission` VALUES ('2', '5', '1', '0');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `tel` varchar(255) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `updateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL,
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0-正常，1-不可用',
  `password` varchar(255) NOT NULL DEFAULT '123456',
  PRIMARY KEY (`id`),
  KEY `idx_name` (`name`),
  KEY `idx_tel` (`tel`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('1', '云云', '506587246@qq.com', '15123850133', '2020-02-12 16:04:18', '2020-02-12 16:04:18', null, '0', '12345678');
INSERT INTO `user` VALUES ('2', '维维', '506587245@qq.com', '15023423412', '2020-02-12 13:09:52', '2020-02-12 13:09:52', null, '0', '123456');

-- ----------------------------
-- Table structure for userandrole
-- ----------------------------
DROP TABLE IF EXISTS `userandrole`;
CREATE TABLE `userandrole` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` bigint(20) NOT NULL,
  `roleId` bigint(20) NOT NULL,
  `status` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idx` (`userId`,`roleId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of userandrole
-- ----------------------------
INSERT INTO `userandrole` VALUES ('1', '1', '1', '0');
INSERT INTO `userandrole` VALUES ('2', '2', '3', '0');
INSERT INTO `userandrole` VALUES ('3', '1', '2', '0');

-- ----------------------------
-- Table structure for userrole
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole` (
  `id` smallint(6) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0-正常，1-不可用',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of userrole
-- ----------------------------
INSERT INTO `userrole` VALUES ('1', 'admin', '0');
INSERT INTO `userrole` VALUES ('2', 'guest', '0');
INSERT INTO `userrole` VALUES ('3', 'common', '0');
