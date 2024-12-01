import React from "react";
import './About.css';
import { ReactComponent as LogoMbtiSvg } from './img/logo-mbti.svg';
import { ReactComponent as StarSvg } from './img/star.svg';
import { ReactComponent as StarsSvg } from './img/stars.svg';
import { ReactComponent as TextSvg } from './img/text.svg';
import { ReactComponent as Text2Svg } from './img/text2.svg';
import { ReactComponent as WaweSvg } from './img/wawe.svg';
import { ReactComponent as Wawe2Svg } from './img/wawe2.svg';
import { ReactComponent as CogniSvg } from './img/COGNI.svg';
import { ReactComponent as TypesSvg } from './img/types.svg';

function About() {
    return (
        <div className="about">
            <div className="about__wrapper">
                <div className="about__banner">
                    <div className="banner__wrapper">
                        <StarSvg></StarSvg>
                        <LogoMbtiSvg></LogoMbtiSvg>
                        <StarSvg></StarSvg>
                        <TextSvg></TextSvg>
                        <WaweSvg></WaweSvg>
                        <CogniSvg></CogniSvg>
                        <TypesSvg></TypesSvg>
                    </div>
                </div>
                <section className="about__description">
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                </section>

                <section className="about__whatIs">
                    <h2 className="whatIs__heading">Что такое MBTI?</h2>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <p>Чтобы узнать больше об MBTI, читайте нашу статью на вики! <a href="#">читать дальше</a></p>
                </section>

                <section className="about__COGNI">
                    <h3 className="COGNI__heading">COGNI - проект Московского Политеха</h3>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <p>Чтобы узнать больше о Московском Политехе, переходите на наш сайт <a href="#">читать дальше</a></p>
                </section>

                <div className="about__banner about__banner--alt">
                    <div className="banner__wrapper banner__wrapper--alt">
                    <StarSvg></StarSvg>
                    <Text2Svg></Text2Svg>
                    <Wawe2Svg></Wawe2Svg>
                    <StarsSvg></StarsSvg>
                    </div>
                </div>

                <section className="about__leaders">
                    <ul className="leaders__list">
                        <li className="leaders__item">
                            <span className="leaders__img"></span>
                            <p className="leaders__name"></p>
                            <span className="leaders__role"></span>
                        </li>
                        <li className="leaders__item">
                            <span className="leaders__img"></span>
                            <p className="leaders__name"></p>
                            <span className="leaders__role"></span>
                        </li>
                        <li className="leaders__item">
                            <span className="leaders__img"></span>
                            <p className="leaders__name"></p>
                            <span className="leaders__role"></span>
                        </li>
                    </ul>
                </section>
            </div>
        </div>
    );
};

export default About;

