--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Postgres.app)
-- Dumped by pg_dump version 17.4 (Postgres.app)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: BuddyUsers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BuddyUsers" (
);


ALTER TABLE public."BuddyUsers" OWNER TO postgres;

--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    password character varying(100) NOT NULL,
    program character varying(150),
    interests text,
    availability character varying(100),
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Data for Name: BuddyUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BuddyUsers"  FROM stdin;
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, name, email, password, program, interests, availability, created_at) FROM stdin;
1	Anna Jensen	anna.j@cbs.dk	Anna2024	Bachelor - International Business	Marketing, Case Studies	Mon-Wed 10:00–13:00	2025-04-29 10:13:45.531582
2	Markus Sørensen	markus.s@cbs.dk	Finance42	Master - Finance and Investments	Excel, Corporate Finance	Tue & Thu 14:00–17:00	2025-04-29 10:13:45.531582
3	Lea Andersen	lea.a@cbs.dk	UXrocks!	Bachelor - Digital Management	UX Design, Startups	Mon-Fri 09:00–12:00	2025-04-29 10:13:45.531582
4	Peter Nielsen	peter.n@cbs.dk	Green2025	Master - Public Management and Social Dev.	Debate, Sustainability	Wed & Fri 13:00–16:00	2025-04-29 10:13:45.531582
5	Sofia Holm	sofia.h@cbs.dk	LawGirl99	Bachelor - Business Admin & Commercial Law	Contracts, Legal Cases	Tue & Thu 11:00–14:00	2025-04-29 10:13:45.531582
6	Emil Kristensen	emil.k@cbs.dk	Data#AI	Master - Business Analytics	Python, AI, Analytics	Mon & Wed 15:00–18:00	2025-04-29 10:13:45.531582
7	Freja Møller	freja.m@cbs.dk	FrejaMktg	Bachelor - Marketing and Communication	Branding, Social Media	Mon-Fri 10:00–13:00	2025-04-29 10:13:45.531582
8	Noah Rasmussen	noah.r@cbs.dk	TaxMan12	Bachelor - Accounting and Finance	Spreadsheets, Tax Law	Tue & Thu 09:00–11:00	2025-04-29 10:13:45.531582
9	Ida Thomsen	ida.t@cbs.dk	StartupQueen	Master - Entrepreneurship and Innovation	Startups, Innovation	Mon-Wed 12:00–15:00	2025-04-29 10:13:45.531582
10	Lucas Lund	lucas.l@cbs.dk	LeadNow!	Master - Strategic Management and Leadership	Strategy, Leadership	Fri 09:00–12:00	2025-04-29 10:13:45.531582
\.


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 10, true);


--
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

