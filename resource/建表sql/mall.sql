/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 80011
Source Host           : localhost:3306
Source Database       : mall

Target Server Type    : MYSQL
Target Server Version : 80011
File Encoding         : 65001

Date: 2020-02-26 17:30:55
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of category
-- ----------------------------
INSERT INTO `category` VALUES ('1', null, '手机', '0', '2020-02-25 15:59:26', '2020-02-25 15:59:26', '2020-02-25 15:59:26');
INSERT INTO `category` VALUES ('2', null, '移动座机', '0', '2020-02-11 15:28:38', '2020-02-11 15:28:38', null);
INSERT INTO `category` VALUES ('3', '1', '滑盖手机', '0', '2020-02-25 14:23:01', '2020-02-25 14:23:01', null);
INSERT INTO `category` VALUES ('4', '1', '折叠手机', '0', '2020-02-25 14:23:12', '2020-02-25 14:23:12', null);
INSERT INTO `category` VALUES ('9', '4', 'test', '0', '2020-02-25 17:08:29', '2020-02-25 17:08:29', null);

-- ----------------------------
-- Table structure for permission
-- ----------------------------
DROP TABLE IF EXISTS `permission`;
CREATE TABLE `permission` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL COMMENT '权限名称',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of permission
-- ----------------------------
INSERT INTO `permission` VALUES ('1', '查询-产品list', '0');
INSERT INTO `permission` VALUES ('2', '更新产品信息', '0');
INSERT INTO `permission` VALUES ('3', '产品上下架', '0');
INSERT INTO `permission` VALUES ('4', '新增商品', '0');
INSERT INTO `permission` VALUES ('5', '查询品类信息', '0');
INSERT INTO `permission` VALUES ('6', '更新品类信息', '0');
INSERT INTO `permission` VALUES ('7', '创建品类信息', '0');
INSERT INTO `permission` VALUES ('8', '查询购物车', '0');
INSERT INTO `permission` VALUES ('9', '修改购物车', '0');

-- ----------------------------
-- Table structure for product
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `categoryId` bigint(20) NOT NULL COMMENT '品类id',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `detail` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT '详情富文本',
  `subtitle` varchar(255) DEFAULT NULL COMMENT '子标题',
  `price` decimal(10,2) NOT NULL COMMENT '商品价格',
  `mainImage` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '商品图片',
  `subImages` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `stock` int(11) NOT NULL DEFAULT '0' COMMENT '库存',
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '商品状态（0-正常，1-不可用）',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `index_name` (`name`) USING BTREE,
  KEY `fk_category` (`categoryId`),
  CONSTRAINT `fk_category` FOREIGN KEY (`categoryId`) REFERENCES `category` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of product
-- ----------------------------
INSERT INTO `product` VALUES ('1', '1', '苹果21', null, '未来科技', '1000.00', 'main.py', null, '200', '0', '2020-02-11 11:28:53', '2020-02-26 14:38:28', null);
INSERT INTO `product` VALUES ('2', '1', 'oppo R8', null, 'oppo促销进行中', '2999.00', 'mainimage2.jpg', null, '5', '0', '2020-02-11 15:33:18', '2020-02-22 18:39:45', null);
INSERT INTO `product` VALUES ('3', '2', '三星折叠手机', null, '爆炸就是艺术', '99999.99', 'baozha.png', 'baozha2.png', '1', '0', '2020-02-25 12:55:45', '2020-02-25 13:28:04', null);

-- ----------------------------
-- Table structure for productcart
-- ----------------------------
DROP TABLE IF EXISTS `productcart`;
CREATE TABLE `productcart` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `userId` bigint(20) NOT NULL,
  `productId` bigint(20) NOT NULL,
  `quantity` int(11) NOT NULL,
  `productSelect` tinyint(4) NOT NULL DEFAULT '1' COMMENT '是否勾选了该商品,1-true ，0-false',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` timestamp NULL DEFAULT NULL,
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0-正常，1-不可用',
  PRIMARY KEY (`id`),
  KEY `index_uid` (`userId`),
  KEY `index_productid` (`productId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of productcart
-- ----------------------------
INSERT INTO `productcart` VALUES ('1', '1', '1', '1', '0', '2020-02-26 14:40:10', '2020-02-26 17:30:04', null, '0');
INSERT INTO `productcart` VALUES ('2', '1', '2', '1', '0', '2020-02-26 15:25:48', '2020-02-26 17:23:00', null, '0');
INSERT INTO `productcart` VALUES ('4', '1', '3', '1', '1', '2020-02-26 17:01:52', '2020-02-26 17:08:38', null, '1');
INSERT INTO `productcart` VALUES ('5', '1', '3', '1', '0', '2020-02-26 17:09:23', '2020-02-26 17:09:44', null, '1');
INSERT INTO `productcart` VALUES ('6', '1', '3', '1', '0', '2020-02-26 17:09:55', '2020-02-26 17:23:00', null, '0');

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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of roleandpermission
-- ----------------------------
INSERT INTO `roleandpermission` VALUES ('1', '4', '1', '0');
INSERT INTO `roleandpermission` VALUES ('2', '5', '1', '0');
INSERT INTO `roleandpermission` VALUES ('3', '1', '2', '0');
INSERT INTO `roleandpermission` VALUES ('4', '1', '3', '0');
INSERT INTO `roleandpermission` VALUES ('5', '1', '4', '0');
INSERT INTO `roleandpermission` VALUES ('6', '1', '5', '0');
INSERT INTO `roleandpermission` VALUES ('7', '1', '6', '0');
INSERT INTO `roleandpermission` VALUES ('8', '1', '7', '0');
INSERT INTO `roleandpermission` VALUES ('9', '1', '8', '0');
INSERT INTO `roleandpermission` VALUES ('10', '1', '9', '0');

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
