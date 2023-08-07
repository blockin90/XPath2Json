<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
		xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:user="urn:my-scripts">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <test2>
      <xsl:value-of select="./field2/text()"/>
    </test2>
    <field1 xsi:nil="true" xsi:type="float"/>
    <emptyObject empty="true"/>
    <emptyStringElement />
    <xsl:for-each select="./objArray">
      <objArray array="true">
        <field1 xsi:type="float">
          <xsl:value-of select="./field1/text()"/>
        </field1>
        <field2>
          <xsl:value-of select="./field2/text()"/>
        </field2>
      </objArray>
    </xsl:for-each>
    <xsl:for-each select="./cars">
      <scalarValue array="true">
        <xsl:value-of select="./text()"/>
      </scalarValue>
    </xsl:for-each>
    <typedArray array="true" xsi:type="float">
      <xsl:text>10.010</xsl:text>
    </typedArray>
    <typedArray array="true" xsi:type="integer">
      <xsl:text>10</xsl:text>
    </typedArray>
    <typedArray array="true" xsi:type="string">
      <xsl:text>str</xsl:text>
    </typedArray>
    <typedArray array="true" xsi:type="date">
      <xsl:text>2020-10-10</xsl:text>
    </typedArray>
    <typedArray array="true" empty="true" xsi:type="date"/>
    <typedArray array="true" empty="true" xsi:type="date"/>
    <eArray emptyArray="true" xsi:type="string"/>
    
  </xsl:template>

  <msxsl:script language="C#" implements-prefix="user">
    <![CDATA[  
  public string RandomNumber(){
    System.Threading.Thread.Sleep(1);
    var value = (new Random()).Next().ToString();
    Console.WriteLine("call RandomNumber");
    return value;
  }  
  ]]>
  </msxsl:script>
</xsl:stylesheet>
