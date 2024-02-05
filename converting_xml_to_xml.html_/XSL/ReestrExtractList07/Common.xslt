<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:include href="Header.xslt"/>
	<xsl:include href="Footer.xslt"/>
	<xsl:include href="Extract.xslt"/>
	<xsl:include href="Notice.xslt"/>
	<xsl:include href="Refusal.xslt"/>
	<xsl:include href="Output.xslt"/>
	<xsl:include href="List.xslt"/>
	<xsl:template match="/">
		<xsl:apply-templates />
	</xsl:template>
</xsl:stylesheet>
